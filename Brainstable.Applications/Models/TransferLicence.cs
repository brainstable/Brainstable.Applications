namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблицы VII.3 «Передача права» (раздел VII):
/// договор о переходе права от одного субъекта к другому.
/// </summary>
public class TransferLicence
{
    /// <summary>Название блока реестра — заголовок верхнего уровня.</summary>
    public string BlockName { get; set; }

    /// <summary>Порядковый номер блока реестра для сортировки.</summary>
    public int BlockOrderNumber { get; set; }

    /// <summary>Русское название рода/вида.</summary>
    public string KindName { get; set; }

    /// <summary>Латинское название рода/вида.</summary>
    public string KindLatineName { get; set; }

    /// <summary>Номер заявки, по которой выдан патент.</summary>
    public int ApplicationId { get; set; }

    /// <summary>Номер патента; выводится в первой колонке.</summary>
    public int PatentNumber { get; set; }

    /// <summary>Идентификатор сорта.</summary>
    public int VarietyId { get; set; }

    /// <summary>Название сорта.</summary>
    public string VarietyName { get; set; }

    /// <summary>Кто передаёт право (идентификатор субъекта).</summary>
    public int WhoId { get; set; }

    /// <summary>Кому передаётся право (идентификатор субъекта).</summary>
    public int WhomId { get; set; }

    /// <summary>Дата регистрации договора.</summary>
    public DateTime Dateregistration { get; set; }

    /// <summary>Номер договора.</summary>
    public string NumberContract { get; set; }

    /// <summary>Вид договора (числовой код).</summary>
    public int TypeContract { get; set; }
}
