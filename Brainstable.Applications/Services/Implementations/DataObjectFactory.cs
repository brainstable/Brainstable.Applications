using Brainstable.Applications.Dto;
using Brainstable.Applications.Infrastructure.Reader;
using Brainstable.Applications.Services.Interfaces;

namespace Brainstable.Applications.Services.Implementations;

/// <summary>
/// Загружает CSV-выгрузку SBAZA через <see cref="LoaderCsv"/>
/// и отдаёт данные в виде DTO и словарей.
/// Путь к каталогу выгрузки задаётся в конструкторе и по умолчанию равен «data/sbaza/».
/// </summary>
public class DataObjectFactory : IDataObjectFactory
{
    private readonly string _dataDirectory;

    /// <summary>
    /// Создаёт фабрику данных.
    /// </summary>
    /// <param name="dataDirectory">
    /// Каталог с CSV-файлами выгрузки. Путь относительный — отсчитывается от текущей рабочей директории.
    /// </param>
    public DataObjectFactory(string dataDirectory = "data/sbaza/")
    {
        _dataDirectory = dataDirectory;
    }

    /// <summary>Собирает путь к файлу выгрузки по его имени.</summary>
    private string GetFilePath(string fileName) => Path.Combine(_dataDirectory, fileName);

    /// <inheritdoc />
    public List<ApplicationDto> CreateApplicationDtos()
    {
        return LoaderCsv.LoadApplicaitons(GetFilePath(DataFileNames.Applications));
    }

    /// <inheritdoc />
    public Dictionary<int, VarietyDto> CreateVarietyDtos()
    {
        return LoaderCsv.LoadVarieties(GetFilePath(DataFileNames.Varieties))
            .ToDictionary(x => x.VarietyId);
    }

    /// <inheritdoc />
    public Dictionary<int, BlockDto> CreateBlockDtos()
    {
        return LoaderCsv.LoadBlocks(GetFilePath(DataFileNames.Blocks))
            .ToDictionary(x => x.Id);
    }

    /// <inheritdoc />
    public Dictionary<int, KindDto> CreateKindDtos()
    {
        return LoaderCsv.LoadKinds(GetFilePath(DataFileNames.Kinds))
            .ToDictionary(x => x.Id);
    }

    /// <inheritdoc />
    public List<ApplicationApplicatorDto> CreateApplicationApplicatorDtos()
    {
        return LoaderCsv.LoadApplicationsApplicators(GetFilePath(DataFileNames.ApplicationApplicators));
    }

    /// <inheritdoc />
    public List<VarietyAutorDto> CreateVarietyAutorDtos()
    {
        return LoaderCsv.LoadVarietiesAutors(GetFilePath(DataFileNames.VarietyAutors));
    }

    /// <inheritdoc />
    public List<VarietyNameApprovedDto> CreateVarietyNameApprovedDtos()
    {
        return LoaderCsv.LoadVarietyNameApprovedDtos(GetFilePath(DataFileNames.VarietySynonyms));
    }

    /// <inheritdoc />
    public Dictionary<int, PatentDto> CreatePatentDtos()
    {
        return LoaderCsv.LoadPatentDtos(GetFilePath(DataFileNames.Patents))
            .ToDictionary(x => x.VarietyId);
    }

    /// <inheritdoc />
    public List<BlockClassDto> CreateBlockClassDtos()
    {
        return LoaderCsv.LoadFromCsvFile(GetFilePath(DataFileNames.BlockClasses));
    }

    /// <inheritdoc />
    public List<SubjectChangeDto> CreateSubjectChangeDtos()
    {
        return LoaderCsv.LoadSubjectChangeDtos(GetFilePath(DataFileNames.SubjectChanges));
    }

    /// <inheritdoc />
    public Dictionary<int, SubjectDto> CreateSubjectDtos()
    {
        return LoaderCsv.LoadSubjects(GetFilePath(DataFileNames.Subjects)).ToDictionary(x => x.Id);
    }

    /// <inheritdoc />
    public List<LicenceDto> CreateLicenceDtos()
    {
        return LoaderCsv.LoadLicenceDtos(GetFilePath(DataFileNames.Licences));
    }

    /// <inheritdoc />
    public List<ContractDto> CreateContractDtos()
    {
        // Для раздела «Передача права» нужны только договоры с патентом и получателем права.
        return LoaderCsv.LoadContractDtos(GetFilePath(DataFileNames.Contracts))
            .Where(x => !string.IsNullOrEmpty(x.PatentId) && x.WhomSubjectId.HasValue)
            .ToList();
    }
}

/// <summary>
/// Имена CSV-файлов выгрузки SBAZA, которые читает приложение.
/// Остальные файлы выгрузки не используются.
/// </summary>
public static class DataFileNames
{
    /// <summary>Заявки на выдачу патента.</summary>
    public const string Applications = "ЗАЯВКА.csv";

    /// <summary>Сорта и гибриды.</summary>
    public const string Varieties = "СОРТ.csv";

    /// <summary>Блоки реестра.</summary>
    public const string Blocks = "БЛОК_РЕЕСТРА.csv";

    /// <summary>Роды/виды растений.</summary>
    public const string Kinds = "РОД.csv";

    /// <summary>Заявители заявок.</summary>
    public const string ApplicationApplicators = "ЗАЯВКА_ЗАЯВИТЕЛЬ.csv";

    /// <summary>Авторы сортов.</summary>
    public const string VarietyAutors = "СОРТ_АВТОР.csv";

    /// <summary>История (синонимы) названий сортов.</summary>
    public const string VarietySynonyms = "СОРТ_СИНОНИМ.csv";

    /// <summary>Патенты на селекционные достижения.</summary>
    public const string Patents = "СОРТ_ПАТЕНТ.csv";

    /// <summary>Блоки классов для сводной таблицы II.5.</summary>
    public const string BlockClasses = "БЛОК_КЛАССОВ.csv";

    /// <summary>Архив изменений состава субъектов.</summary>
    public const string SubjectChanges = "АРХИВ_СД.csv";

    /// <summary>Субъекты (заявители, авторы, патентообладатели).</summary>
    public const string Subjects = "СУБЪЕКТ.csv";

    /// <summary>Договоры передачи права.</summary>
    public const string Contracts = "ДОГОВОР.csv";

    /// <summary>Лицензии.</summary>
    public const string Licences = "ЛИЦЕНЗИЯ.csv";

    /// <summary>Открытые лицензии; в текущей версии не используются.</summary>
    public const string LicencesOpen = "ЛИЦЕНЗИЯ_ОТКРЫТА.csv";
}
