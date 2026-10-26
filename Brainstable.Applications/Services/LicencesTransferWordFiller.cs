using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// Заполнитель таблицы VII.3 «Передача права» (раздел VII).
/// Выводит стороны договора, его номер, дату регистрации и вид.
/// Шаблон: «7.4 Передача права.docx».
/// </summary>
public class LicencesTransferWordFiller : WordTemplateFillerBase<TransferLicence>
{
    // Ширина колонок в процентах
    private const string Column1Width = "9";  // Номер заявки
    private const string Column2Width = "23";  // Заявитель/Автор
    private const string Column3Width = "13";  // Название/Селекционный номер
    private const string Column4Width = "13";  // Дата регистрации
    private const string Column5Width = "16";  // Дата регистрации
    private const string Column6Width = "13";  // Дата регистрации
    private const string Column7Width = "13";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, TransferLicence>> GroupItems(List<TransferLicence> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, TransferLicence> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, TransferLicence>> GetSubGroups(IGrouping<object, TransferLicence> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, TransferLicence> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<TransferLicence> GetDataItems(IGrouping<object, TransferLicence> subGroup)
    {
        return subGroup.OrderBy(x => x.VarietyName);
    }

    /// <summary>
    /// Определяет структуру таблицы: 7 колонок
    /// </summary>
    protected override List<TableColumnDefinition> GetTableColumns()
    {
        return new List<TableColumnDefinition>
        {
            new TableColumnDefinition("Number", Column1Width, "Номер заявки"),
            new TableColumnDefinition("Variety", Column2Width, "Заявитель/Автор"),
            new TableColumnDefinition("Who", Column3Width, "Дата регистрации"),
            new TableColumnDefinition("Whom", Column4Width, "Название/Селекционный номер"),
            new TableColumnDefinition("DateRegistration", Column5Width, "Название/Селекционный номер"),
            new TableColumnDefinition("NumberRegistration", Column6Width, "Название/Селекционный номер"),
            new TableColumnDefinition("TypeLicence", Column7Width, "Дата регистрации")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(TransferLicence item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.PatentNumber.ToString() };
            case "Who":
                return new List<string> { item.WhoId.ToString() };
            case "Whom":
                return new List<string> { item.WhomId.ToString() };
            case "NumberRegistration":
                return new List<string> { item.NumberContract };
            case "DateRegistration":
                return new List<string> { item.Dateregistration.ToString("dd.MM.yyyy") };
            case "Variety":
                return new List<string> { item.VarietyName };
            case "TypeLicence":
                return new List<string> { item.TypeContract.ToString() };
            default:
                return new List<string>();
        }
    }

    /// <summary>
    /// Настраивает форматирование для ячеек:
    /// - Для названия сорта - подчеркивание первой строки
    /// - Для селекционного номера - курсив второй строки
    /// - Для данных о заявителе/авторе - мелкий шрифт
    /// </summary>
    protected override CellFormatting GetCellFormatting(TransferLicence item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        if (columnName == "SubjectIds")
        {
            // Название сорта - подчеркнутое
            //formatting.LineFormatting[0] = new CellLineFormatting { IsBold = true };
            // Селекционный номер - курсив
            //formatting.LineFormatting[1] = new CellLineFormatting { };
            //formatting.FontSize = FontSizeSmall;
        }
        //else if (columnName == "Applicant")
        //{
        //    // Данные о заявителе/авторе - мелкий шрифт
        //    formatting.FontSize = FontSizeSmall;
        //}

        return formatting;
    }
}