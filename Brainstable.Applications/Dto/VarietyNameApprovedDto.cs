namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы СОРТ_СИНОНИМ — история названий сорта: предыдущее (одобренное)
/// название, дата его изменения и дата одобрения нового названия.
/// На этих данных строятся разделы II.1, II.3 и II.4.
/// </summary>
public record VarietyNameApprovedDto
{
    /// <summary>Идентификатор сорта (К_СОРТА).</summary>
    public int VarietyId { get; set; }

    /// <summary>Ранее одобренное (предыдущее) название сорта (И_СОРТА).</summary>
    public string ApprovedName { get; set; }

    /// <summary>Дата одобрения нового названия (ДАТА_ОДОБРЕНИЯ), может отсутствовать.</summary>
    public DateTime? DateApproved { get; set; }

    /// <summary>Дата изменения названия (ДАТА_ИЗМЕНЕНИЯ).</summary>
    public DateTime DateChanged { get; set; }
}
