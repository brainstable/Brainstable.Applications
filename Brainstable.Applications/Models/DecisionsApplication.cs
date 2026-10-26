namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы IV.1 «Сорта и гибриды, включённые в Государственный реестр
/// охраняемых селекционных достижений» (раздел IV), то есть сорта, получившие патент.
/// </summary>
public class DecisionsApplication
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

    /// <summary>Название сорта.</summary>
    public string VarietyName { get; set; }

    /// <summary>Селекционный номер сорта.</summary>
    public string SelectionNumber { get; set; }

    /// <summary>Номер заявки на охрану.</summary>
    public int ApplicationAdguardId { get; set; }

    /// <summary>Заявители через запятую (подпункт «а»).</summary>
    public string ApplicatorIds { get; set; }

    /// <summary>Авторы через запятую (подпункт «б»).</summary>
    public string AutorIds { get; set; }

    /// <summary>Дата начала действия патента (дата регистрации).</summary>
    public DateTime DateRegistrationPatent { get; set; }

    /// <summary>Номер патента.</summary>
    public int NumberPatent { get; set; }
}
