namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы ДОГОВОР — договор о передаче права на патент (таблица VII.3).
/// </summary>
public class ContractDto
{
    /// <summary>Номер связанной заявки (Н_ЗАЯВКИ).</summary>
    public int ApplicationId { get; set; }

    /// <summary>Номер патента (Н_ПАТЕНТА); в выгрузке хранится строкой.</summary>
    public string PatentId { get; set; }

    /// <summary>Дата регистрации договора (ДАТА_РЕГИСТРАЦИИ).</summary>
    public DateTime DateRegistration { get; set; }

    /// <summary>Вид договора (ВИД), выводится в колонке «Вид лицензии».</summary>
    public int TypeLicence { get; set; }

    /// <summary>Номер договора (Н_ДОГОВОРА).</summary>
    public string ContractNumber { get; set; }

    /// <summary>Условия договора (УСЛОВИЯ), в бюллетене не выводятся.</summary>
    public string Conditions { get; set; }

    /// <summary>Кто передаёт право — идентификатор субъекта (КТО), может отсутствовать.</summary>
    public int? WhoSubjectId { get; set; }

    /// <summary>Кому передаётся право — идентификатор субъекта (КОМУ), может отсутствовать.</summary>
    public int? WhomSubjectId { get; set; }

    /// <summary>Дата аннулирования договора (ДАТА_АННУЛИРОВАНИЯ), может отсутствовать.</summary>
    public DateTime? DateAnnulation { get; set; }

    /// <summary>Дата окончания действия договора (ДАТА_К_ДЕЙСТВИЯ), может отсутствовать.</summary>
    public DateTime? DateExpiration { get; set; }

    /// <summary>Идентификатор сорта (К_СОРТА).</summary>
    public int VarietyId { get; set; }
}
