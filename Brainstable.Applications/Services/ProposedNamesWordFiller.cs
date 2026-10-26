using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// Заполнитель таблицы II.1 «Предложенные названия» (раздел II).
/// Показывает предыдущее и предложенное название, селекционный номер и дату предложения.
/// Шаблон: «2.1 Предложенные названия.docx».
/// </summary>
public class ProposedNamesWordFiller : WordTemplateFillerBase<ProposedName>
{
    // Ширина колонок в процентах
    private const string Column1Width = "12";  // Номер заявки
    private const string Column2Width = "24";  // Заявитель/Автор
    private const string Column3Width = "32";  // Название/Селекционный номер
    private const string Column4Width = "32";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ProposedName>> GroupItems(List<ProposedName> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, ProposedName> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ProposedName>> GetSubGroups(IGrouping<object, ProposedName> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, ProposedName> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<ProposedName> GetDataItems(IGrouping<object, ProposedName> subGroup)
    {
        return subGroup.OrderBy(x => x.PreviousVarietyName);
    }

    /// <summary>
    /// Определяет структуру таблицы: 4 колонки
    /// </summary>
    protected override List<TableColumnDefinition> GetTableColumns()
    {
        return new List<TableColumnDefinition>
        {
            new TableColumnDefinition("Number", Column1Width, "Номер заявки"),
            new TableColumnDefinition("Applicant", Column2Width, "Заявитель/Автор"),
            new TableColumnDefinition("PreviousVariety", Column3Width, "Предыдущее название/Селекционный номер"),
            new TableColumnDefinition("ProposedVarietyName", Column4Width, "Принятое название/Дата принятия")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(ProposedName item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.VarietyId.ToString() };
            case "Applicant":
                var result = new List<string> { $"{item.ApplicatorIds}" };
                return result;
            case "PreviousVariety":
                return new List<string> { item.PreviousVarietyName, item.SelectionNumber };
            case "ProposedVarietyName":
                return new List<string> { item.ProposedVarietyName, item.DateProposed.ToString("dd.MM.yyyy") };
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
    protected override CellFormatting GetCellFormatting(ProposedName item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        if (columnName == "PreviousVariety")
        {
            // Название сорта - подчеркнутое
            formatting.LineFormatting[0] = new CellLineFormatting { IsUnderline = true };
            // Селекционный номер - курсив
            formatting.LineFormatting[1] = new CellLineFormatting { IsItalic = true };
        }
        else if (columnName == "Applicant")
        {
            // Данные о заявителе/авторе - мелкий шрифт
            formatting.FontSize = FontSizeSmall;
        }

        return formatting;
    }
}