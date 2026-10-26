using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// Заполнитель таблицы II.2 «Одобренные названия» (раздел II).
/// Выводит сорт, впервые включённый в реестр охраняемых, и его заявителей.
/// Шаблон: «2.2 Одобренные названия.docx».
/// </summary>
public class ApprovedNamesWordFiller : WordTemplateFillerBase<ApprovedName>
{
    // Ширина колонок в процентах
    private const string Column1Width = "12";  // Номер заявки
    private const string Column2Width = "33";  // Заявитель/Автор
    private const string Column3Width = "55";  // Название/Селекционный номер

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ApprovedName>> GroupItems(List<ApprovedName> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, ApprovedName> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ApprovedName>> GetSubGroups(IGrouping<object, ApprovedName> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, ApprovedName> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<ApprovedName> GetDataItems(IGrouping<object, ApprovedName> subGroup)
    {
        return subGroup.OrderBy(x => x.VarietyName);
    }

    /// <summary>
    /// Определяет структуру таблицы: 3 колонки
    /// </summary>
    protected override List<TableColumnDefinition> GetTableColumns()
    {
        return new List<TableColumnDefinition>
        {
            new TableColumnDefinition("Number", Column1Width, "Номер заявки"),
            new TableColumnDefinition("Applicant", Column2Width, "Заявитель/Автор"),
            new TableColumnDefinition("Variety", Column3Width, "Одобренное название"),
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(ApprovedName item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.VarietyId.ToString() };
            case "Applicant":
                var result = new List<string> { $"{item.ApplicatorIds}" };
                return result;
            case "Variety":
                return new List<string> { item.VarietyName };
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
    protected override CellFormatting GetCellFormatting(ApprovedName item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        //if (columnName == "Applicant")
        //{
        //    // Данные о заявителе/авторе - мелкий шрифт
        //    formatting.FontSize = FontSizeSmall;
        //}

        return formatting;
    }
}