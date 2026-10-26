using Brainstable.Applications.Dto;

namespace Brainstable.Applications.Services.Interfaces;

/// <summary>
/// Фабрика объектов данных: загружает CSV-выгрузку и возвращает DTO,
/// сгруппированные так, как удобно выборкам разделов бюллетеня.
/// </summary>
public interface IDataObjectFactory
{
    /// <summary>Читает заявки типа 1 (заявки на выдачу патента).</summary>
    List<ApplicationDto> CreateApplicationDtos();

    /// <summary>Читает сорта и индексирует их по идентификатору сорта.</summary>
    Dictionary<int, VarietyDto> CreateVarietyDtos();

    /// <summary>Читает блоки реестра и индексирует их по идентификатору блока.</summary>
    Dictionary<int, BlockDto> CreateBlockDtos();

    /// <summary>Читает роды/виды и индексирует их по идентификатору рода.</summary>
    Dictionary<int, KindDto> CreateKindDtos();

    /// <summary>Читает список заявителей заявок.</summary>
    List<ApplicationApplicatorDto> CreateApplicationApplicatorDtos();

    /// <summary>Читает список авторов сортов.</summary>
    List<VarietyAutorDto> CreateVarietyAutorDtos();

    /// <summary>Читает историю названий сортов (СОРТ_СИНОНИМ).</summary>
    List<VarietyNameApprovedDto> CreateVarietyNameApprovedDtos();

    /// <summary>Читает патенты и индексирует их по идентификатору сорта.</summary>
    Dictionary<int, PatentDto> CreatePatentDtos();

    /// <summary>Читает блоки классов для сводной таблицы II.5.</summary>
    List<BlockClassDto> CreateBlockClassDtos();

    /// <summary>Читает изменения состава субъектов.</summary>
    List<SubjectChangeDto> CreateSubjectChangeDtos();

    /// <summary>Читает субъектов и индексирует их по идентификатору.</summary>
    Dictionary<int, SubjectDto> CreateSubjectDtos();

    /// <summary>Читает лицензии.</summary>
    List<LicenceDto> CreateLicenceDtos();

    /// <summary>
    /// Читает договоры передачи права и оставляет только строки,
    /// у которых заполнены номер патента и получатель права.
    /// </summary>
    List<ContractDto> CreateContractDtos();
}
