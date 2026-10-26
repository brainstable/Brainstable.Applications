namespace Brainstable.Applications.Models;

/// <summary>
/// Строка таблиц VII.1 «Зарегистрированные лицензии» и VII.2 «Аннулированные лицензии»:
/// один патент (сорт) и все связанные с ним лицензии.
/// </summary>
public class Licence
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

    /// <summary>Патентообладатели через запятую (подпункт «а»).</summary>
    public string PatentownerIds { get; set; }

    /// <summary>Версия записи; в бюллетене не используется.</summary>
    public int? Version { get; set; }

    /// <summary>Лицензии, выданные по этому патенту.</summary>
    public List<LicenceItem> LicenceItems { get; set; } = [];
}

/// <summary>
/// Одна лицензия в составе строки таблицы VII.1 или VII.2.
/// </summary>
public class LicenceItem
{
    /// <summary>Идентификатор лицензиата (субъекта, получившего право).</summary>
    public int LicenseeId { get; set; }

    /// <summary>Дата регистрации лицензии.</summary>
    public DateTime DateRegistration { get; set; }

    /// <summary>Дата аннулирования лицензии; заполняется для таблицы VII.2.</summary>
    public DateTime? DateAnnulation { get; set; }

    /// <summary>Дата окончания действия лицензии.</summary>
    public DateTime? DateExpiration { get; set; }

    /// <summary>Регистрационный номер лицензии.</summary>
    public string NumberLicence { get; set; }

    /// <summary>Вид лицензии в текстовом виде (исключительная, неисключительная, открытая, принудительная).</summary>
    public string TypeLicence { get; set; }
}
