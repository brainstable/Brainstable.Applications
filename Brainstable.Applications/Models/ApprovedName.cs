namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы II.2 «Одобренные названия» (раздел II):
/// название сорта, впервые включённого в реестр охраняемых селекционных достижений.
/// </summary>
public class ApprovedName
{
    /// <summary>Название блока реестра — заголовок верхнего уровня.</summary>
    public string BlockName { get; set; }

    /// <summary>Порядковый номер блока реестра для сортировки.</summary>
    public int BlockOrderNumber { get; set; }

    /// <summary>Русское название рода/вида.</summary>
    public string KindName { get; set; }

    /// <summary>Латинское название рода/вида.</summary>
    public string KindLatineName { get; set; }

    /// <summary>Идентификатор сорта; выводится в колонке «Номер заявки».</summary>
    public int VarietyId { get; set; }

    /// <summary>Одобренное название сорта.</summary>
    public string VarietyName { get; set; }

    /// <summary>Заявители через запятую.</summary>
    public string ApplicatorIds { get; set; }
}
