namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы II.4 «Одобренные изменения названий» (раздел II).
/// </summary>
public class ApprovedChangeApprovedName
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

    /// <summary>Имеющееся (прежнее) название сорта.</summary>
    public string PreviousVarietyName { get; set; }

    /// <summary>Новое одобренное название сорта.</summary>
    public string ProposedVarietyName { get; set; }

    /// <summary>Селекционный номер сорта.</summary>
    public string SelectionNumber { get; set; }

    /// <summary>Дата одобрения нового названия.</summary>
    public DateTime DateProposed { get; set; }

    /// <summary>Дата изменения названия.</summary>
    public DateTime DateChanged { get; set; }

    /// <summary>Заявители через запятую.</summary>
    public string ApplicatorIds { get; set; }

    /// <summary>Номер патента на сорт.</summary>
    public int PatentNumber { get; set; }
}
