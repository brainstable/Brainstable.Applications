namespace Brainstable.Applications.Dto;

/// <summary>
/// Строка таблицы СОРТ — селекционное достижение (сорт или гибрид).
/// </summary>
public record VarietyDto
{
    /// <summary>Идентификатор сорта (К_СОРТА), ключ словаря сортов.</summary>
    public int VarietyId { get; set; }

    /// <summary>Название сорта (И_СОРТА).</summary>
    public string VarietyName { get; set; }

    /// <summary>Селекционный номер (Н_СЕЛЕКЦИОННЫЙ), может отсутствовать.</summary>
    public string SelectionNumber { get; set; }

    /// <summary>Латинское название сорта (И_СОРТА_ЛАТ); в бюллетене не выводится.</summary>
    public string LatineName { get; set; }

    /// <summary>Идентификатор рода/вида (Н_РОДА).</summary>
    public int KindId { get; set; }
}
