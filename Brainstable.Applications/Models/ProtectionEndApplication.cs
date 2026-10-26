namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы VI.1 «Окончание охраны» (раздел VI):
/// сорт, у которого прекращается действие патента.
/// </summary>
public class ProtectionEndApplication
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

    /// <summary>Номер патента.</summary>
    public int PatentId { get; set; }

    /// <summary>Название сорта.</summary>
    public string VarietyName { get; set; }

    /// <summary>Селекционный номер сорта.</summary>
    public string SelectionNumber { get; set; }

    /// <summary>Номер заявки на охрану.</summary>
    public int ApplicationAdguardId { get; set; }

    /// <summary>Заявители через запятую.</summary>
    public string ApplicatorIds { get; set; }

    /// <summary>Дата окончания охраны.</summary>
    public DateTime? DateAdguardEnd { get; set; }

    /// <summary>Код причины окончания охраны.</summary>
    public int CauseEnd { get; set; }
}
