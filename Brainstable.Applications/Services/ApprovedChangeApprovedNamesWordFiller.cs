using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// 2.2 Одобренные названия
/// </summary>
public class ApprovedChangeApprovedNamesWordFiller : WordTemplateFillerBase<ApprovedChangeApprovedName>
{
    // Ширина колонок в процентах
    private const string Column1Width = "12";  // Номер заявки
    private const string Column2Width = "24";  // Заявитель/Автор
    private const string Column3Width = "32";  // Название/Селекционный номер
    private const string Column4Width = "32";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ApprovedChangeApprovedName>> GroupItems(List<ApprovedChangeApprovedName> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, ApprovedChangeApprovedName> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, ApprovedChangeApprovedName>> GetSubGroups(IGrouping<object, ApprovedChangeApprovedName> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, ApprovedChangeApprovedName> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<ApprovedChangeApprovedName> GetDataItems(IGrouping<object, ApprovedChangeApprovedName> subGroup)
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
            new TableColumnDefinition("PreviousVariety", Column3Width, "Имеющееся название/Селекционный номер"),
            new TableColumnDefinition("ProposedVariety", Column4Width, "Предложенное новое название/Дата предложения")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(ApprovedChangeApprovedName item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.VarietyId.ToString() };
            case "Applicant":
                var result = new List<string> { $"{item.ApplicatorIds}" };
                return result;
            case "PreviousVariety":
                return new List<string> { item.PreviousVarietyName };
            case "ProposedVariety":
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
    protected override CellFormatting GetCellFormatting(ApprovedChangeApprovedName item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        //if (columnName == "PreviousVariety")
        //{
        //    // Название сорта - подчеркнутое
        //    formatting.LineFormatting[0] = new CellLineFormatting { IsUnderline = true };
        //    // Селекционный номер - курсив
        //    formatting.LineFormatting[1] = new CellLineFormatting { IsItalic = true };
        //}
        //else if (columnName == "Applicant")
        //{
        //    // Данные о заявителе/авторе - мелкий шрифт
        //    formatting.FontSize = FontSizeSmall;
        //}

        return formatting;
    }
}