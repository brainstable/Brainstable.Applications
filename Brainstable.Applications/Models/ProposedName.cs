namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы II.1 «Предложенные названия» (раздел II):
/// сведения о предложенном названии сорта и его предыдущем названии.
/// </summary>
public class ProposedName
{
    /// <summary>Название блока реестра — заголовок верхнего уровня.</summary>
    public string BlockName { get; set; }

    /// <summary>Порядковый номер блока реестра для сортировки.</summary>
    public int BlockOrderNumber { get; set; }

    /// <summary>Русское название рода/вида.</summary>
    public string KindName { get; set; }

    /// <summary>Латинское название рода/вида.</summary>
    public string KindLatineName { get; set; }

    /// <summary>Идентификатор сорта.</summary>
    public int VarietyId { get; set; }

    /// <summary>Предыдущее (одобренное) название сорта.</summary>
    public string PreviousVarietyName { get; set; }

    /// <summary>Предложенное название сорта.</summary>
    public string ProposedVarietyName { get; set; }

    /// <summary>Селекционный номер сорта.</summary>
    public string SelectionNumber { get; set; }

    /// <summary>Дата предложения названия.</summary>
    public DateTime DateProposed { get; set; }

    /// <summary>Дата изменения названия.</summary>
    public DateTime DateChanged { get; set; }

    /// <summary>Заявители через запятую.</summary>
    public string ApplicatorIds { get; set; }
}
