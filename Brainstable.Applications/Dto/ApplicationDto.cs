namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы ЗАЯВКА — заявка на выдачу патента на селекционное достижение.
/// Соответствие колонкам выгрузки задаётся в <c>ApplicationMap</c>.
/// </summary>
public record ApplicationDto
{
    /// <summary>Номер заявки (Н_ЗАЯВКИ).</summary>
    public int Id { get; set; }

    /// <summary>Идентификатор сорта, к которому относится заявка (К_СОРТА).</summary>
    public int VarietyId { get; set; }

    /// <summary>Тип заявки (Н_ТИПА_ЗАЯВКИ). В разделах бюллетеня используются только заявки типа 1.</summary>
    public int Type { get; set; }

    /// <summary>Дата регистрации заявки (ДАТА_РЕГИСТРАЦИИ).</summary>
    public DateTime DateRegistration { get; set; }

    /// <summary>Дата отклонения заявки (ДАТА_ОТКЛОНЕНИЯ), может отсутствовать.</summary>
    public DateTime? DateRejection { get; set; }

    /// <summary>
    /// Код причины отклонения (Н_ПРИЧ_ОТКЛОНЕНИЯ):
    /// 1 — отзыв заявки, 2/3/5/7 — отклонение заявки.
    /// </summary>
    public int? CauseRejection { get; set; }

    /// <summary>Дата экспертизы заявки (ДАТА_ЭКСПЕРТИЗЫ), может отсутствовать.</summary>
    public DateTime? DateExpertise { get; set; }

    /// <summary>Идентификатор заявителя из самой заявки (Н_СУБЪЕКТА_П). Имя свойства сохранено с опечаткой.</summary>
    public int ApplicaitorId { get; set; }

    /// <summary>Дата включения сведений о заявке (ДАТА_ВКЛЮЧЕНИЯ), может отсутствовать.</summary>
    public DateTime? DateInclusion { get; set; }

    /// <summary>Код причины окончания охраны (Н_ПРИЧ_К_ОХР_ДОП), может отсутствовать.</summary>
    public int? NumberCaudeAdguardEnd { get; set; }
}
