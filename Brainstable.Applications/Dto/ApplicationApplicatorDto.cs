namespace Brainstable.Applications.Dto;

/// <summary>
/// Связь заявки с заявителем: одна строка таблицы ЗАЯВКА_ЗАЯВИТЕЛЬ.
/// У заявки может быть несколько заявителей, порядок их вывода задаётся приоритетом.
/// </summary>
public record ApplicationApplicatorDto
{
    /// <summary>Номер заявки (Н_ЗАЯВКИ).</summary>
    public int ApplicationId { get; set; }

    /// <summary>Идентификатор субъекта-заявителя (Н_СУБЪЕКТА).</summary>
    public int ApplicatorId { get; set; }

    /// <summary>Приоритет вывода заявителя: чем меньше значение, тем раньше заявитель в списке (ПРИОРИТЕТ).</summary>
    public int Priority { get; set; }
}
