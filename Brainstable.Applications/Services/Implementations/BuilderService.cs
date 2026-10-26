using Brainstable.Applications.Dto;
using Brainstable.Applications.Infrastructure.Reader;
using Brainstable.Applications.Models;
using Brainstable.Applications.Services.Interfaces;

using DocumentFormat.OpenXml.InkML;

namespace Brainstable.Applications.Services.Implementations;

/// <summary>
/// Формирует модели разделов бюллетеня из загруженных DTO: фильтрует записи по периоду выпуска,
/// подставляет названия блока реестра и рода, объединяет заявителей и авторов
/// и накапливает список субъектов для раздела VIII.
///
/// Все выборки используют полуоткрытый интервал: firstDate &lt; дата &lt;= lastDate,
/// то есть начальная дата в период не входит, конечная входит.
/// </summary>
internal class BuilderService
{
    private readonly IDataObjectFactory _factory;

    // Загруженные таблицы выгрузки; заполняются в Load().
    private List<ApplicationDto> applicationDtos = [];
    private Dictionary<int, VarietyDto> varietyDtos = [];
    private Dictionary<int, BlockDto> blockDtos = [];
    private Dictionary<int, KindDto> kindDtos = [];
    private List<ApplicationApplicatorDto> applicationApplicatorDtos = [];
    private List<VarietyAutorDto> varietyAutorDtos = [];
    private List<VarietyNameApprovedDto> varietyNameApprovedDto = [];
    private Dictionary<int, PatentDto> patentDtos = [];
    private List<BlockClassDto> blockClassDtos  = [];
    private List<SubjectChangeDto> subjectChangeDtos = [];

    // Идентификаторы субъектов, встретившихся в разделах, и пары для сводной таблицы II.5.
    private HashSet<int> _subjectIds = [];
    HashSet<BlockClassVarietyName> _blockClassVarietyNames = [];

    private Dictionary<int, SubjectDto> subjectDtos = [];
    private List<LicenceDto> licenceDtos = [];
    private List<ContractDto> contractDtos = [];

    /// <summary>Создаёт построитель разделов на основе фабрики данных.</summary>
    public BuilderService(IDataObjectFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Загружает все CSV-таблицы, необходимые для разделов бюллетеня.
    /// Вызывается один раз перед построением выпуска.
    /// </summary>
    public void Load()
    {
        applicationDtos = _factory.CreateApplicationDtos();
        varietyDtos = _factory.CreateVarietyDtos();
        blockDtos = _factory.CreateBlockDtos();
        kindDtos = _factory.CreateKindDtos();
        applicationApplicatorDtos = _factory.CreateApplicationApplicatorDtos();
        varietyAutorDtos = _factory.CreateVarietyAutorDtos();
        varietyNameApprovedDto = _factory.CreateVarietyNameApprovedDtos();
        patentDtos = _factory.CreatePatentDtos();
        blockClassDtos = _factory.CreateBlockClassDtos();
        subjectChangeDtos = _factory.CreateSubjectChangeDtos();
        subjectDtos = _factory.CreateSubjectDtos();
        licenceDtos = _factory.CreateLicenceDtos();
        contractDtos = _factory.CreateContractDtos();
    }

    /// <summary>
    /// Запоминает субъектов, встретившихся в разделе, чтобы затем вывести их в таблице VIII.1.
    /// </summary>
    private void AddSubjects(IEnumerable<int> subjectIds)
    {
        foreach (var subjectId in subjectIds)
        {
            _subjectIds.Add(subjectId);
        }
    }

    /// <summary>
    /// Возвращает накопленные пары «блок классов — название сорта»
    /// для сводной таблицы II.5.
    /// </summary>
    public List<BlockClassVarietyName> GetBlockClassVarietyNames()
    {
        return _blockClassVarietyNames.ToList();
    }

    /// <summary>
    /// Раздел I, таблица I.1 — принятые заявки: дата экспертизы попала в период,
    /// заявка не отклонена и имеет тип 1.
    /// </summary>
    public List<AcceptedApplication> GetAcceptedApplications(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<AcceptedApplication>();

        foreach (var dto in applicationDtos.Where(x => x.DateExpertise.HasValue && !x.DateRejection.HasValue && x.DateExpertise.Value > firstDate && x.DateExpertise <= lastDate && x.Type == 1))
        {
            var aa = new AcceptedApplication();

            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var autors = varietyAutorDtos.Where(x => x.VarietyId == dto.VarietyId).Select(x => x.AutorId).OrderBy(x => x);
            var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == dto.Id).OrderBy(x => x.Priority)
                .Select(x => x.ApplicatorId);

            AddSubjects(autors);
            AddSubjects(applicators);

            aa.VarietyId = dto.VarietyId;
            aa.ApplicationAdguardId = dto.Id;
            aa.ApplicatorIds = string.Join(", ", applicators);
            aa.BlockName = block.Name;
            aa.BlockOrderNumber = block.OrderNumber;
            aa.KindName = kind.Name;
            aa.KindLatineName = kind.LatineName;
            aa.VarietyName = variety.VarietyName;
            aa.SelectionNumber = variety.SelectionNumber;
            aa.DateAdguardRegistration = dto.DateRegistration;
            aa.DateAdguardExpertise = dto.DateExpertise;
            aa.AutorIds = string.Join(", ", autors);

            var blockClass = blockClassDtos.FirstOrDefault(x => x.Id == kind.ClassId);
            if (blockClass is not null)
            {
                var bc = new BlockClassVarietyName();
                bc.BlockClassId = blockClass.Id;
                bc.BlockClassName = blockClass.Name;
                bc.VarietyName = aa.VarietyName;

                _blockClassVarietyNames.Add(bc);
            }

            list.Add(aa);
        }

        return list;
    }

    /// <summary>
    /// Раздел II, таблица II.1 — предложенные названия: дата изменения названия
    /// попала в период и по сорту есть заявка типа 1.
    /// </summary>
    public List<ProposedName> GetProposedNames(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<ProposedName>();

        foreach (var dto in varietyNameApprovedDto.Where(x => x.DateChanged > firstDate && x.DateChanged <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);
            if (application is not null)
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);

                AddSubjects(applicators);

                var pn = new ProposedName();
                pn.VarietyId = dto.VarietyId;
                pn.PreviousVarietyName = dto.ApprovedName;
                pn.ProposedVarietyName = variety.VarietyName;
                pn.SelectionNumber = variety.SelectionNumber;
                pn.DateChanged = dto.DateChanged;
                pn.DateProposed = dto.DateApproved ?? dto.DateChanged.AddDays(1);
                pn.BlockName = block.Name;
                pn.BlockOrderNumber = block.OrderNumber;
                pn.KindName = kind.Name;
                pn.KindLatineName = kind.LatineName;
                pn.ApplicatorIds = string.Join(", ", applicators);

                var blockClass = blockClassDtos.FirstOrDefault(x => x.Id == kind.ClassId);
                if (blockClass is not null)
                {
                    var bc = new BlockClassVarietyName();
                    bc.BlockClassId = blockClass.Id;
                    bc.BlockClassName = blockClass.Name;
                    bc.VarietyName = pn.ProposedVarietyName;

                    _blockClassVarietyNames.Add(bc);
                }

                list.Add(pn);
            }
        }

        return list;
    }

    /// <summary>
    /// Раздел II, таблица II.2 — одобренные названия сортов, впервые включённых
    /// в реестр охраняемых: дата начала охраны попала в период.
    /// </summary>
    public List<ApprovedName> GetApprovedNames(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<ApprovedName>();

        foreach (var dto in patentDtos.Values.Where(x => x.DateStart > firstDate && x.DateStart <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);
            if (application is not null)
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);
                
                AddSubjects(applicators);

                var pn = new ApprovedName();
                pn.VarietyId = dto.VarietyId;
                pn.VarietyName = variety.VarietyName;
                pn.BlockName = block.Name;
                pn.BlockOrderNumber = block.OrderNumber;
                pn.KindName = kind.Name;
                pn.KindLatineName = kind.LatineName;
                pn.ApplicatorIds = string.Join(", ", applicators);

                var blockClass = blockClassDtos.FirstOrDefault(x => x.Id == kind.ClassId);
                if (blockClass is not null)
                {
                    var bc = new BlockClassVarietyName();
                    bc.BlockClassId = blockClass.Id;
                    bc.BlockClassName = blockClass.Name;
                    bc.VarietyName = pn.VarietyName;

                    _blockClassVarietyNames.Add(bc);
                }

                list.Add(pn);
            }
        }

        return list;
    }

    /// <summary>
    /// Раздел II, таблица II.3 — предложения по изменению одобренных названий:
    /// дата изменения попала в период, по сорту есть патент и охрана началась
    /// не позже конца периода.
    /// </summary>
    public List<ProposalsChangeApprovedName> GetProposalsChangeApprovedNames(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<ProposalsChangeApprovedName>();

        foreach (var dto in varietyNameApprovedDto.Where(x => x.DateChanged > firstDate && x.DateChanged <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];

            if (!patentDtos.ContainsKey(variety.VarietyId))
            {
                continue;
            }

            var patent = patentDtos[variety.VarietyId];

            if (patent.DateStart > lastDate)
            {
                continue;
            }

            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);
            if (application is not null)
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);

                AddSubjects(applicators);

                var pn = new ProposalsChangeApprovedName();
                pn.VarietyId = dto.VarietyId;
                pn.PreviousVarietyName = dto.ApprovedName;
                pn.ProposedVarietyName = variety.VarietyName;
                pn.SelectionNumber = variety.SelectionNumber;
                pn.DateChanged = dto.DateChanged;
                pn.DateProposed = dto.DateApproved ?? dto.DateChanged.AddDays(1);
                pn.BlockName = block.Name;
                pn.BlockOrderNumber = block.OrderNumber;
                pn.KindName = kind.Name;
                pn.KindLatineName = kind.LatineName;
                pn.PatentNumber = patent.NumberPatent;
                pn.ApplicatorIds = string.Join(", ", applicators);

                var blockClass = blockClassDtos.FirstOrDefault(x => x.Id == kind.ClassId);
                if (blockClass is not null)
                {
                    var bc = new BlockClassVarietyName();
                    bc.BlockClassId = blockClass.Id;
                    bc.BlockClassName = blockClass.Name;
                    bc.VarietyName = pn.ProposedVarietyName;

                    _blockClassVarietyNames.Add(bc);
                }

                list.Add(pn);
            }
        }

        return list;
    }

    /// <summary>
    /// Раздел II, таблица II.4 — одобренные изменения названий: дата одобрения
    /// попала в период, по сорту есть патент и охрана началась не позже конца периода.
    /// </summary>
    public List<ApprovedChangeApprovedName> GetApprovedChangeApprovedNames(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<ApprovedChangeApprovedName>();

        foreach (var dto in varietyNameApprovedDto.Where(x => x.DateApproved > firstDate && x.DateApproved <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];

            if (!patentDtos.ContainsKey(variety.VarietyId))
            {
                continue;
            }

            var patent = patentDtos[variety.VarietyId];

            if (patent.DateStart > lastDate)
            {
                continue;
            }

            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);
            if (application is not null)
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);

                AddSubjects(applicators);

                var pn = new ApprovedChangeApprovedName();
                pn.VarietyId = dto.VarietyId;
                pn.PreviousVarietyName = dto.ApprovedName;
                pn.ProposedVarietyName = variety.VarietyName;
                pn.SelectionNumber = variety.SelectionNumber;
                pn.DateChanged = dto.DateChanged;
                pn.DateProposed = dto.DateApproved ?? dto.DateChanged.AddDays(1);
                pn.BlockName = block.Name;
                pn.BlockOrderNumber = block.OrderNumber;
                pn.KindName = kind.Name;
                pn.KindLatineName = kind.LatineName;
                pn.PatentNumber = patent.NumberPatent;
                pn.ApplicatorIds = string.Join(", ", applicators);

                var blockClass = blockClassDtos.FirstOrDefault(x => x.Id == kind.ClassId);
                if (blockClass is not null)
                {
                    var bc = new BlockClassVarietyName();
                    bc.BlockClassId = blockClass.Id;
                    bc.BlockClassName = blockClass.Name;
                    bc.VarietyName = pn.ProposedVarietyName;

                    _blockClassVarietyNames.Add(bc);
                }

                list.Add(pn);
            }
        }

        return list;
    }

    /// <summary>
    /// Раздел III, таблица III.1 — отозванные заявки: дата отклонения попала в период,
    /// причина отклонения равна 1 (отзыв), тип заявки — 1.
    /// </summary>
    public List<WithdrawnApplication> GetWithdrawnApplications(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<WithdrawnApplication>();

        foreach (var dto in applicationDtos.Where(x => x.DateRejection.HasValue 
                                                       && x.DateRejection.Value > firstDate 
                                                         && x.DateRejection <= lastDate 
                                                         && x.CauseRejection.HasValue 
                                                         && x.CauseRejection.Value == 1
                                                         && x.Type == 1))
        {
            var aa = new WithdrawnApplication();

            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var autors = varietyAutorDtos.Where(x => x.VarietyId == dto.VarietyId).Select(x => x.AutorId).OrderBy(x => x);
            var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == dto.Id).OrderBy(x => x.Priority)
                .Select(x => x.ApplicatorId);

            AddSubjects(autors);
            AddSubjects(applicators);

            aa.VarietyId = dto.VarietyId;
            aa.ApplicationAdguardId = dto.Id;
            aa.ApplicatorIds = string.Join(", ", applicators);
            aa.BlockName = block.Name;
            aa.BlockOrderNumber = block.OrderNumber;
            aa.KindName = kind.Name;
            aa.KindLatineName = kind.LatineName;
            aa.VarietyName = variety.VarietyName;
            aa.SelectionNumber = variety.SelectionNumber;
            aa.DateAdguardWithdrawal = dto.DateRejection!.Value;
            aa.CauseWithdrawal = dto.CauseRejection!.Value;
            aa.AutorIds = string.Join(", ", autors);

            list.Add(aa);
        }

        return list;
    }

    /// <summary>
    /// Раздел IV, таблица IV.1 — сорта и гибриды, включённые в реестр охраняемых:
    /// дата начала действия патента попала в период.
    /// </summary>
    public List<DecisionsApplication> GetDecisionsApplications(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<DecisionsApplication>();

        foreach (var dto in patentDtos.Values.Where(x => x.DateStart > firstDate && x.DateStart <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);
            if (application is not null)
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);
                var autors = varietyAutorDtos.Where(x => x.VarietyId == dto.VarietyId).Select(x => x.AutorId).OrderBy(x => x);

                AddSubjects(autors);
                AddSubjects(applicators);

                var pn = new DecisionsApplication();
                pn.VarietyId = dto.VarietyId;
                pn.VarietyName = variety.VarietyName;
                pn.BlockName = block.Name;
                pn.BlockOrderNumber = block.OrderNumber;
                pn.KindName = kind.Name;
                pn.KindLatineName = kind.LatineName;
                pn.ApplicatorIds = string.Join(", ", applicators);
                pn.AutorIds = string.Join(", ", autors);
                pn.ApplicationAdguardId = application.Id;
                pn.DateRegistrationPatent = dto.DateStart;
                pn.NumberPatent = dto.NumberPatent;
                pn.SelectionNumber = variety.SelectionNumber;

                list.Add(pn);
            }
        }

        return list;
    }

    /// <summary>
    /// Раздел IV, таблица IV.2 — отклонённые заявки на выдачу патента:
    /// дата отклонения попала в период, причина отклонения — 2, 3, 5 или 7.
    /// </summary>
    public List<RejectionApplication> GetRejectionApplications(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<RejectionApplication>();

        foreach (var dto in applicationDtos.Where(x => x.DateRejection.HasValue
                                                       && x.DateRejection.Value > firstDate
                                                       && x.DateRejection <= lastDate
                                                       && x.CauseRejection.HasValue
                                                       && (x.CauseRejection.Value == 2
                                                       || x.CauseRejection.Value == 3
                                                       || x.CauseRejection.Value == 5
                                                       || x.CauseRejection.Value == 7)
                                                       && x.Type == 1))
        {
            var aa = new RejectionApplication();

            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var autors = varietyAutorDtos.Where(x => x.VarietyId == dto.VarietyId).Select(x => x.AutorId).OrderBy(x => x);
            var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == dto.Id).OrderBy(x => x.Priority)
                .Select(x => x.ApplicatorId);

            AddSubjects(autors);
            AddSubjects(applicators);

            aa.VarietyId = dto.VarietyId;
            aa.ApplicationAdguardId = dto.Id;
            aa.ApplicatorIds = string.Join(", ", applicators);
            aa.BlockName = block.Name;
            aa.BlockOrderNumber = block.OrderNumber;
            aa.KindName = kind.Name;
            aa.KindLatineName = kind.LatineName;
            aa.VarietyName = variety.VarietyName;
            aa.SelectionNumber = variety.SelectionNumber;
            aa.DateAdguardRejection = dto.DateRejection!.Value;
            aa.CauseWithdrawal = dto.CauseRejection!.Value;
            aa.AutorIds = string.Join(", ", autors);

            list.Add(aa);
        }

        return list;
    }

    /// <summary>
    /// Раздел V, таблица V.1 — изменения состава субъектов селекционных достижений.
    /// Строки группируются по сорту; изменения субъектов (замена, добавление, удаление)
    /// накапливаются в <see cref="SubjectChanged.SubjectChangedItems"/> без дубликатов.
    /// </summary>
    public List<SubjectChanged> GetSubjectChangeds(DateTime firstDate, DateTime lastDate)
    {
        var dict = new Dictionary<int, SubjectChanged>();

        // Отбираем изменения по заявкам на патент; тип субъекта 5 в раздел не включается.
        foreach (var dto in subjectChangeDtos.Where(x => x.DateChanged > firstDate
                 && x.DateChanged <= lastDate && x.TypeApplication == 1
                 && x.TypeSubject != 5))
        {
            // Первая запись по сорту создаёт строку раздела, остальные дополняют её.
            if (!dict.ContainsKey(dto.VarietyId))
            {
                var variety = varietyDtos[dto.VarietyId];
                var kind = kindDtos[variety.KindId];
                var block = blockDtos[kind.BlockId];

                var aa = new SubjectChanged();
                aa.VarietyId = dto.VarietyId;
                aa.ApplicationId = dto.ApplicationId;
                aa.BlockName = block.Name;
                aa.BlockOrderNumber = block.OrderNumber;
                aa.KindName = kind.Name;
                aa.KindLatineName = kind.LatineName;
                aa.VarietyName = variety.VarietyName;

                dict[dto.VarietyId] = aa;
            }

            // заменен
            if (dto.CauseChange == 1)
            {
                if (dto.OldSubjectId.HasValue && dto.NewSubjectId.HasValue)
                {
                    var item = new SubjectChangedItem();

                    item.IncludedSubjectId = $"{dto.NewSubjectId.Value}";
                    item.ExcludedSubjectId = $"{dto.OldSubjectId.Value}";
                    item.TypeSubject = dto.TypeSubject;
                    item.DateChanged = dto.DateChanged;

                    AddSubjects([dto.OldSubjectId.Value, dto.NewSubjectId.Value]);

                    var exists = false;
                    foreach (var ci in dict[dto.VarietyId].SubjectChangedItems.Where(ci => ci.Equals(item)))
                    {
                        exists = true;
                    }

                    if (!exists)
                    {
                        dict[dto.VarietyId].SubjectChangedItems.Add(item);
                    }
                }
            }

            // добавлен
            if (dto.CauseChange == 2)
            {
                if (dto.NewSubjectId.HasValue)
                {
                    var item = new SubjectChangedItem();

                    item.IncludedSubjectId = $"{dto.NewSubjectId.Value}";
                    item.ExcludedSubjectId = "";
                    item.TypeSubject = dto.TypeSubject;
                    item.DateChanged = dto.DateChanged;

                    AddSubjects([dto.NewSubjectId.Value]);

                    var exists = false;
                    foreach (var ci in dict[dto.VarietyId].SubjectChangedItems.Where(ci => ci.Equals(item)))
                    {
                        exists = true;
                    }

                    if (!exists)
                    {
                        dict[dto.VarietyId].SubjectChangedItems.Add(item);
                    }
                }
            }

            // удален
            if (dto.CauseChange == 3)
            {
                if (dto.OldSubjectId.HasValue)
                {
                    var item = new SubjectChangedItem();

                    item.ExcludedSubjectId = $"{dto.OldSubjectId.Value}";
                    item.IncludedSubjectId = "";
                    item.TypeSubject = dto.TypeSubject;
                    item.DateChanged = dto.DateChanged;

                    AddSubjects([dto.OldSubjectId.Value]);

                    var exists = false;
                    foreach (var ci in dict[dto.VarietyId].SubjectChangedItems.Where(ci => ci.Equals(item)))
                    {
                        exists = true;
                    }

                    if (!exists)
                    {
                        dict[dto.VarietyId].SubjectChangedItems.Add(item);
                    }
                }
            }
        }

        return dict.Values.ToList();
    }

    /// <summary>
    /// Раздел VIII, таблица VIII.1 — список субъектов, встретившихся при построении
    /// остальных разделов выпуска.
    /// </summary>
    public List<SubjectName> GetSubjectNames()
    {
        var list = new List<SubjectName>();

        foreach (var subjectId in _subjectIds)
        {
            var subject = subjectDtos[subjectId];

            list.Add(new SubjectName
            {
                Id = subjectId,
                Name = subject.Name
            });
        }

        return list;
    }

    /// <summary>
    /// Раздел VII, таблица VII.1 — зарегистрированные лицензии: дата регистрации
    /// попала в период и лицензия ещё не аннулирована.
    /// </summary>
    public List<Licence> GetLicences(DateTime firstDate, DateTime lastDate)
    {
        // Ключ словаря — номер патента: все лицензии одного патента выводятся одной строкой.
        var dict = new Dictionary<int, Licence>();

        foreach (var dto in licenceDtos.Where(x => x.DateRegistration > firstDate && x.DateRegistration <= lastDate && !x.DateAnnulation.HasValue))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);

            if (application is null)
                continue;

            if (!dict.ContainsKey(dto.PatentId))
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);

                AddSubjects(applicators);

                var licence = new Licence();
                licence.VarietyName = variety.VarietyName;
                licence.VarietyId = variety.VarietyId;
                licence.ApplicationId = dto.ApplicationId;
                licence.BlockName = block.Name;
                licence.BlockOrderNumber = block.OrderNumber;
                licence.KindLatineName = kind.LatineName;
                licence.KindName = kind.Name;
                licence.PatentNumber = dto.PatentId;
                licence.PatentownerIds = string.Join(", ", applicators);

                dict[dto.PatentId] = licence;
            }

            var li = new LicenceItem();
            li.DateRegistration = dto.DateRegistration;
            li.DateAnnulation = dto.DateAnnulation;
            li.NumberLicence = dto.NumberLicence;
            li.LicenseeId = dto.SubjectId;

            AddSubjects([dto.SubjectId]);

            li.TypeLicence = dto.TypeLicence switch
            {
                1 => "исключительная",
                2 => "неисключительная",
                3 => "открытая",
                4 => "принудительная",
                _ => li.TypeLicence
            };

            dict[dto.PatentId].LicenceItems.Add(li);
        }

        return dict.Values.ToList();
    }

    /// <summary>
    /// Раздел VII, таблица VII.2 — аннулированные лицензии: дата аннулирования
    /// попала в период.
    /// </summary>
    public List<Licence> GetCanceledLicences(DateTime firstDate, DateTime lastDate)
    {
        var dict = new Dictionary<int, Licence>();

        // Для аннулированных лицензий признак — дата аннулирования в периоде.
        foreach (var dto in licenceDtos.Where(x => x.DateAnnulation.HasValue && x.DateAnnulation > firstDate && x.DateAnnulation <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);

            if (application is null)
                continue;

            if (!dict.ContainsKey(dto.PatentId))
            {
                var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                    .Select(x => x.ApplicatorId);

                AddSubjects([]);

                var licence = new Licence();
                licence.VarietyName = variety.VarietyName;
                licence.VarietyId = variety.VarietyId;
                licence.ApplicationId = dto.ApplicationId;
                licence.BlockName = block.Name;
                licence.BlockOrderNumber = block.OrderNumber;
                licence.KindLatineName = kind.LatineName;
                licence.KindName = kind.Name;
                licence.PatentNumber = dto.PatentId;
                licence.PatentownerIds = string.Join(", ", applicators);

                dict[dto.PatentId] = licence;
            }

            var li = new LicenceItem();
            li.DateRegistration = dto.DateRegistration;
            li.DateAnnulation = dto.DateAnnulation;
            li.NumberLicence = dto.NumberLicence;
            li.LicenseeId = dto.SubjectId;

            AddSubjects([dto.SubjectId]);

            li.TypeLicence = dto.TypeLicence switch
            {
                1 => "исключительная",
                2 => "неисключительная",
                3 => "открытая",
                4 => "принудительная",
                _ => li.TypeLicence
            };

            dict[dto.PatentId].LicenceItems.Add(li);
        }

        return dict.Values.ToList();
    }

    /// <summary>
    /// Раздел VII, таблица VII.3 — передача права: дата регистрации договора
    /// попала в период и договор не аннулирован.
    /// </summary>
    public List<TransferLicence> GetTransferLicences(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<TransferLicence>();

        foreach (var dto in contractDtos.Where(x => x.DateRegistration > firstDate && x.DateRegistration <= lastDate && !x.DateAnnulation.HasValue))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);

            if (application is null)
                continue;

            var licence = new TransferLicence();
            licence.VarietyName = variety.VarietyName;
            licence.VarietyId = variety.VarietyId;
            licence.ApplicationId = dto.ApplicationId;
            licence.BlockName = block.Name;
            licence.BlockOrderNumber = block.OrderNumber;
            licence.KindLatineName = kind.LatineName;
            licence.KindName = kind.Name;
            licence.PatentNumber = Convert.ToInt32(dto.PatentId);
            licence.WhoId = dto.WhoSubjectId!.Value;
            licence.WhomId = dto.WhomSubjectId!.Value;
            licence.Dateregistration = dto.DateRegistration;
            licence.NumberContract = dto.ContractNumber;
            licence.TypeContract = dto.TypeLicence;

            AddSubjects([licence.WhoId, licence.WhomId]);

            list.Add(licence);
        }

        return list;
    }

    /// <summary>
    /// Раздел VI, таблица VI.1 — окончание охраны: дата окончания действия патента
    /// попала в период.
    /// </summary>
    public List<ProtectionEndApplication> GetProtectionEndApplications(DateTime firstDate, DateTime lastDate)
    {
        var list = new List<ProtectionEndApplication>();

        foreach (var dto in patentDtos.Values.Where(x => x.DateEnd.HasValue && x.DateEnd > firstDate && x.DateEnd <= lastDate))
        {
            var variety = varietyDtos[dto.VarietyId];
            var kind = kindDtos[variety.KindId];
            var block = blockDtos[kind.BlockId];
            var application = applicationDtos.FirstOrDefault(x => x.VarietyId == dto.VarietyId && x.Type == 1);

            if (application is null)
                continue;

            var applicators = applicationApplicatorDtos.Where(x => x.ApplicationId == application!.Id).OrderBy(x => x.Priority)
                .Select(x => x.ApplicatorId);

            AddSubjects(applicators);

            var protectionEndApplication = new ProtectionEndApplication();
            protectionEndApplication.VarietyName = variety.VarietyName;
            protectionEndApplication.VarietyId = variety.VarietyId;
            protectionEndApplication.ApplicationAdguardId = application.ApplicaitorId;
            protectionEndApplication.BlockName = block.Name;
            protectionEndApplication.BlockOrderNumber = block.OrderNumber;
            protectionEndApplication.KindLatineName = kind.LatineName;
            protectionEndApplication.KindName = kind.Name;
            protectionEndApplication.PatentId = dto.NumberPatent;
            protectionEndApplication.ApplicatorIds = string.Join(", ", applicators);
            protectionEndApplication.CauseEnd = application.NumberCaudeAdguardEnd!.Value;
            protectionEndApplication.DateAdguardEnd = dto.DateEnd;

            list.Add(protectionEndApplication);
        }

        return list;
    }
}