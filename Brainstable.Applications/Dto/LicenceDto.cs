namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы ЛИЦЕНЗИЯ — зарегистрированная или аннулированная лицензия
/// на использование селекционного достижения (таблицы VII.1 и VII.2).
/// </summary>
public class LicenceDto
{
    /// <summary>Номер связанной заявки (Н_ЗАЯВКИ).</summary>
    public int ApplicationId { get; set; }

    /// <summary>Номер патента (Н_ПАТЕНТА).</summary>
    public int PatentId { get; set; }

    /// <summary>Дата регистрации лицензии (ДАТА_РЕГИСТРАЦИИ).</summary>
    public DateTime DateRegistration { get; set; }

    /// <summary>Регистрационный номер лицензии (Н_ЛИЦЕНЗИИ).</summary>
    public string NumberLicence { get; set; }

    /// <summary>
    /// Вид лицензии (ВИД): 1 — исключительная, 2 — неисключительная,
    /// 3 — открытая, 4 — принудительная.
    /// </summary>
    public int TypeLicence { get; set; }

    /// <summary>Идентификатор сорта (К_СОРТА).</summary>
    public int VarietyId { get; set; }

    /// <summary>Дата аннулирования лицензии (ДАТА_АННУЛИРОВАНИЯ), может отсутствовать.</summary>
    public DateTime? DateAnnulation { get; set; }

    /// <summary>Дата окончания действия лицензии (ДАТА_К_ДЕЙСТВИЯ), может отсутствовать.</summary>
    public DateTime? DateExpiration { get; set; }

    /// <summary>Идентификатор лицензиата — субъекта, получившего право (Н_СУБЪЕКТА).</summary>
    public int SubjectId { get; set; }

    /// <summary>Версия записи (ВЕРСИЯ), в бюллетене не используется.</summary>
    public int? Version { get; set; }
}
