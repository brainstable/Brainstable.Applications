namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы СУБЪЕКТ — заявитель, автор или патентообладатель.
/// Используется для расшифровки идентификаторов субъектов в разделе VIII.1.
/// </summary>
public record SubjectDto
{
    /// <summary>Номер субъекта (Н_СУБЪЕКТА).</summary>
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор субъекта в ФГИС. В текущей реализации читается из той же колонки 0,
    /// что и <see cref="Id"/>, и в отчётах не используется.
    /// </summary>
    public int FgisId { get; set; }

    /// <summary>Наименование субъекта (И_СУБЪЕКТА).</summary>
    public string Name { get; set; }

    /// <summary>ИНН субъекта; в бюллетене не выводится.</summary>
    public string Inn { get; set; }

    /// <summary>СНИЛС субъекта; в бюллетене не выводится.</summary>
    public string Snils { get; set; }
}
