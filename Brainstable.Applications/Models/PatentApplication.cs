namespace Brainstable.Applications.Models;

/// <summary>
/// Промежуточная модель заявки на выдачу патента.
/// В текущей версии не используется: разделы строятся сразу из DTO.
/// </summary>
internal class PatentApplication
{
    /// <summary>Название блока реестра.</summary>
    public required string BlockName { get; set; }

    /// <summary>Русское название рода/вида.</summary>
    public required string KindName { get; set; }

    /// <summary>Латинское название рода/вида.</summary>
    public required string KindLatineName { get; set; }

    /// <summary>Идентификатор сорта.</summary>
    public int VarietyId { get; set; }

    /// <summary>Название сорта.</summary>
    public required string VarietyName { get; set; }

    /// <summary>Селекционный номер сорта.</summary>
    public string? SelectionNumber { get; set; }

    /// <summary>Номер заявки на допуск.</summary>
    public int ApplicationAdmissionId { get; set; }

    /// <summary>Номер заявки на охрану.</summary>
    public int ApplicationAdguardId { get; set; }

    /// <summary>Идентификатор заявителя.</summary>
    public int ApplicatorId { get; set; }

    /// <summary>Наименование заявителя.</summary>
    public string ApplicatorName { get; set; }

    /// <summary>Авторы через запятую.</summary>
    public string AutorIds { get; set; }

    /// <summary>Оригинаторы через запятую.</summary>
    public string OriginatorIds { get; set; }

    /// <summary>Дата отзыва заявки на охрану.</summary>
    public DateTime? DateAdguardWithdrawal { get; set; }

    /// <summary>Дата отзыва заявки на допуск.</summary>
    public DateTime? DateAddmissionWithdrawal { get; set; }

    /// <summary>Дата экспертизы заявки на охрану.</summary>
    public DateTime? DateAdguardExpertise { get; set; }

    /// <summary>Дата экспертизы заявки на допуск.</summary>
    public DateTime? DateAddmissionExpertise { get; set; }
}
