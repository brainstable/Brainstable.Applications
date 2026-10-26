namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы I.1 «Заявки на выдачу патента» (раздел I).
/// Содержит сведения о заявке, принятой к рассмотрению.
/// </summary>
public class AcceptedApplication
{
    /// <summary>Название блока реестра — заголовок верхнего уровня.</summary>
    public string BlockName { get; set; }

    /// <summary>Порядковый номер блока реестра для сортировки разделов.</summary>
    public int BlockOrderNumber { get; set; }

    /// <summary>Русское название рода/вида.</summary>
    public string KindName { get; set; }

    /// <summary>Латинское название рода/вида.</summary>
    public string KindLatineName { get; set; }

    /// <summary>Идентификатор сорта; в текущей вёрстке выводится в колонке «Номер заявки».</summary>
    public int VarietyId { get; set; }

    /// <summary>Название сорта.</summary>
    public string VarietyName { get; set; }

    /// <summary>Селекционный номер сорта.</summary>
    public string SelectionNumber { get; set; }

    /// <summary>Номер заявки на охрану (внутренний идентификатор заявки).</summary>
    public int ApplicationAdguardId { get; set; }

    /// <summary>Заявители через запятую (подпункт «а»), в порядке приоритета.</summary>
    public string ApplicatorIds { get; set; }

    /// <summary>Авторы через запятую (подпункт «б»), по возрастанию идентификатора.</summary>
    public string AutorIds { get; set; }

    // Оригинаторы в текущую версию бюллетеня не выводятся.
    //public string OriginatorIds { get; set; }

    /// <summary>Дата регистрации заявки.</summary>
    public DateTime? DateAdguardRegistration { get; set; }

    /// <summary>Дата экспертизы заявки; в таблицу не выводится, используется для отбора.</summary>
    public DateTime? DateAdguardExpertise { get; set; }

    // Зарезервированные поля для будущих разделов:
    //// +) заявка опубликована ранее
    //public bool PublicationAddmissionEarlier { get; set; }
    //public bool PublicationAdguardEarlier { get; set; }

    //// ++) заявка не подана
    //public bool NotAddmissionSubmitted { get; set; }
    //public bool NotAdguardSubmitted { get; set; }

    //// *) Предоставляется временная правовая охрана?
    //public int ApplicationAdmissionId { get; set; }
    //public DateTime? DateAddmissionRegistration { get; set; }
    //public DateTime? DateAddmissionExpertise { get; set; }
}
