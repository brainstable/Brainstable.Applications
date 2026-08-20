using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// 1.1 Заявки на выдачу патента
/// </summary>
public class SubjectChangedWordFiller : WordTemplateFillerBase<SubjectChanged>
{
    // Ширина колонок в процентах
    private const string Column1Width = "12";  // Номер заявки
    private const string Column2Width = "28";  // Заявитель/Автор
    private const string Column3Width = "40";  // Название/Селекционный номер
    private const string Column4Width = "20";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, SubjectChanged>> GroupItems(List<SubjectChanged> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, SubjectChanged> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, SubjectChanged>> GetSubGroups(IGrouping<object, SubjectChanged> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, SubjectChanged> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<SubjectChanged> GetDataItems(IGrouping<object, SubjectChanged> subGroup)
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
            new TableColumnDefinition("Variety", Column2Width, "Заявитель/Автор"),
            new TableColumnDefinition("OldSubject", Column3Width, "Название/Селекционный номер"),
            new TableColumnDefinition("NewSubject", Column3Width, "Название/Селекционный номер"),
            new TableColumnDefinition("Date", Column4Width, "Дата регистрации")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(SubjectChanged item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.VarietyId.ToString() };
            case "OldSubject":
                var result = new List<string>();
                foreach (var ci in item.SubjectChangedItems)
                {
                    if (ci.TypeSubject == 1)
                    {
                        if (!string.IsNullOrEmpty(ci.ExcludedSubjectId))
                        {
                            result.Add($"в: {ci.ExcludedSubjectId}");
                        }
                        else
                        {
                            result.Add("");
                        }
                    }
                    if (ci.TypeSubject == 2)
                    {
                        if (!string.IsNullOrEmpty(ci.ExcludedSubjectId))
                        {
                            result.Add($"в: {ci.ExcludedSubjectId}");
                        }
                        else
                        {
                            result.Add("");
                        }
                    }
                    if (ci.TypeSubject == 6)
                    {
                        if (!string.IsNullOrEmpty(ci.ExcludedSubjectId))
                        {
                            result.Add($"в: {ci.ExcludedSubjectId}");
                        }
                        else
                        {
                            result.Add("");
                        }
                    }
                }
                return result;
            case "NewSubject":
                var result2 = new List<string>();
                foreach (var ci in item.SubjectChangedItems)
                {
                    if (ci.TypeSubject == 1)
                    {
                        if (!string.IsNullOrEmpty(ci.IncludedSubjectId))
                        {
                            result2.Add($"в: {ci.IncludedSubjectId}");
                        }
                        else
                        {
                            result2.Add("");
                        }
                    }
                    if (ci.TypeSubject == 2)
                    {
                        if (!string.IsNullOrEmpty(ci.IncludedSubjectId))
                        {
                            result2.Add($"в: {ci.IncludedSubjectId}");
                        }
                        else
                        {
                            result2.Add("");
                        }
                    }
                    if (ci.TypeSubject == 6)
                    {
                        if (!string.IsNullOrEmpty(ci.IncludedSubjectId))
                        {
                            result2.Add($"в: {ci.IncludedSubjectId}");
                        }
                        else
                        {
                            result2.Add("");
                        }
                    }
                }
                return result2;
            case "Variety":
                return new List<string> { item.VarietyName };
            case "Date":
                var result3 = new List<string>();
                foreach (var ci in item.SubjectChangedItems)
                {
                    if (ci.TypeSubject == 1)
                    {
                        result3.Add($"{ci.DateChanged.ToString("dd.MM.yyyy")}");
                    }
                    if (ci.TypeSubject == 2)
                    {
                        result3.Add($"{ci.DateChanged.ToString("dd.MM.yyyy")}");
                    }
                    if (ci.TypeSubject == 6)
                    {
                        result3.Add($"{ci.DateChanged.ToString("dd.MM.yyyy")}");
                    }
                }
                return result3;
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
    protected override CellFormatting GetCellFormatting(SubjectChanged item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        //if (columnName == "Variety")
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