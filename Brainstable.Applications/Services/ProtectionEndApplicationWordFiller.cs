using Brainstable.Applications.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

/// <summary>
/// 6.1 Окончание охраны
/// </summary>
public class ProtectionEndApplicationWordFiller : WordTemplateFillerBase<ProtectionEndApplication>
{
    // Ширина колонок в процентах
    private const string Column1Width = "8";  // Номер заявки
    private const string Column2Width = "11";  // Заявитель/Автор
    private const string Column3Width = "17";  // Название/Селекционный номер
    private const string Column4Width = "28";  // Дата регистрации
    private const string Column5Width = "25";  // Дата регистрации
    private const string Column6Width = "11";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ProtectionEndApplication>> GroupItems(List<ProtectionEndApplication> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, ProtectionEndApplication> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ProtectionEndApplication>> GetSubGroups(IGrouping<object, ProtectionEndApplication> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, ProtectionEndApplication> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<ProtectionEndApplication> GetDataItems(IGrouping<object, ProtectionEndApplication> subGroup)
    {
        return subGroup.OrderBy(x => x.VarietyName);
    }

    /// <summary>
    /// Определяет структуру таблицы: 4 колонки
    /// </summary>
    protected override List<TableColumnDefinition> GetTableColumns()
    {
        return new List<TableColumnDefinition>
        {
            new TableColumnDefinition("Number", Column1Width, "Номер заявки"),
            new TableColumnDefinition("Patent", Column2Width, "Номер заявки"),
            new TableColumnDefinition("Applicant", Column3Width, "Заявитель/Автор"),
            new TableColumnDefinition("Variety", Column4Width, "Название/Селекционный номер"),
            new TableColumnDefinition("Date", Column5Width, "Дата регистрации"),
            new TableColumnDefinition("Cause", Column6Width, "Дата регистрации")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(ProtectionEndApplication item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.VarietyId.ToString() };
            case "Patent":
                return new List<string> { item.PatentId.ToString() };
            case "Applicant":
                return new List<string> { $"{item.ApplicatorIds}" };
            case "Variety":
                return new List<string> { item.VarietyName, item.SelectionNumber };
            case "Date":
                return new List<string> { item.DateAdguardEnd?.ToString("dd.MM.yyyy") ?? "" };
            case "Cause":
                return new List<string> { $"{item.CauseEnd}" };
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
    protected override CellFormatting GetCellFormatting(ProtectionEndApplication item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        if (columnName == "Datey")
        {
            formatting.Alignment = new EnumValue<JustificationValues>(JustificationValues.Center);
        }

        return formatting;
    }
}