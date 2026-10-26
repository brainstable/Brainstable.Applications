using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// Заполнитель таблицы IV.2 «Отклонённые заявки на выдачу патента» (раздел IV).
/// В колонке даты дополнительно выводится код причины отклонения в квадратных скобках.
/// Шаблон: «4.6 Отклоненные заявки на выдачу патента.docx».
/// </summary>
public class RejectionApplicationWordFiller : WordTemplateFillerBase<RejectionApplication>
{
    // Ширина колонок в процентах
    private const string Column1Width = "12";  // Номер заявки
    private const string Column2Width = "28";  // Заявитель/Автор
    private const string Column3Width = "40";  // Название/Селекционный номер
    private const string Column4Width = "20";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, RejectionApplication>> GroupItems(List<RejectionApplication> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, RejectionApplication> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, RejectionApplication>> GetSubGroups(IGrouping<object, RejectionApplication> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, RejectionApplication> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<RejectionApplication> GetDataItems(IGrouping<object, RejectionApplication> subGroup)
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
            new TableColumnDefinition("Applicant", Column2Width, "Заявитель/Автор"),
            new TableColumnDefinition("Variety", Column3Width, "Название/Селекционный номер"),
            new TableColumnDefinition("Date", Column4Width, "Дата регистрации")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(RejectionApplication item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.VarietyId.ToString() };
            case "Applicant":
                var result = new List<string> { $"а: {item.ApplicatorIds}" };
                if (!string.IsNullOrEmpty(item.AutorIds))
                {
                    result.Add($"б: {item.AutorIds}");
                }
                return result;
            case "Variety":
                return new List<string> { item.VarietyName, item.SelectionNumber };
            case "Date":
                // Дата отклонения и код причины в квадратных скобках.
                return new List<string> { item.DateAdguardRejection.ToString("dd.MM.yyyy"), $"[ {item.CauseWithdrawal} ]" };
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
    protected override CellFormatting GetCellFormatting(RejectionApplication item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        if (columnName == "Variety")
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