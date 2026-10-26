namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы IV.2 «Отклонённые заявки на выдачу патента» (раздел IV).
/// </summary>
public class RejectionApplication
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

    /// <summary>Дата отклонения заявки.</summary>
    public DateTime DateAdguardRejection { get; set; }

    /// <summary>Код причины отклонения; выводится в квадратных скобках после даты.</summary>
    public int CauseWithdrawal { get; set; }
}
