using Brainstable.Applications.Models;

namespace Brainstable.Applications.Services;

/// <summary>
/// 7.1 Зарегистрированные лицензии
/// </summary>
public class LicencesWordFiller : WordTemplateFillerBase<Licence>
{
    // Ширина колонок в процентах
    private const string Column1Width = "9";  // Номер заявки
    private const string Column2Width = "15";  // Заявитель/Автор
    private const string Column3Width = "21";  // Название/Селекционный номер
    private const string Column4Width = "16";  // Дата регистрации
    private const string Column5Width = "16";  // Дата регистрации
    private const string Column6Width = "21";  // Дата регистрации

    /// <summary>
    /// Группирует заявки по блокам (культурам)
    /// </summary>
    protected override IEnumerable<IGrouping<object, Licence>> GroupItems(List<Licence> items)
    {
        return items
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);
    }

    /// <summary>
    /// Возвращает название блока (культуры) как заголовок группы
    /// </summary>
    protected override string GetGroupHeader(IGrouping<object, Licence> group)
    {
        return ((dynamic)group.Key).BlockName;
    }

    /// <summary>
    /// Группирует заявки внутри блока по видам (ботаническим названиям)
    /// </summary>
    protected override IEnumerable<IGrouping<object, Licence>> GetSubGroups(IGrouping<object, Licence> group)
    {
        return group
            .GroupBy(a => new { a.KindName, a.KindLatineName })
            .OrderBy(g => g.Key.KindName);
    }

    /// <summary>
    /// Возвращает русское и латинское название вида
    /// </summary>
    protected override string GetSubGroupHeader(IGrouping<object, Licence> subGroup)
    {
        var key = (dynamic)subGroup.Key;
        return $"{key.KindName}\n{key.KindLatineName}";
    }

    /// <summary>
    /// Возвращает заявки, отсортированные по названию сорта
    /// </summary>
    protected override IEnumerable<Licence> GetDataItems(IGrouping<object, Licence> subGroup)
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
            new TableColumnDefinition("SubjectIds", Column2Width, "Номер заявки"),
            new TableColumnDefinition("Variety", Column3Width, "Заявитель/Автор"),
            new TableColumnDefinition("DateRegistration", Column4Width, "Название/Селекционный номер"),
            new TableColumnDefinition("NumberRegistration", Column5Width, "Название/Селекционный номер"),
            new TableColumnDefinition("TypeLicence", Column6Width, "Дата регистрации")
        };
    }

    /// <summary>
    /// Формирует данные для каждой колонки
    /// </summary>
    protected override List<string> GetCellData(Licence item, string columnName)
    {
        switch (columnName)
        {
            case "Number":
                return new List<string> { item.PatentNumber.ToString() };
            case "SubjectIds":
                var subjectIds = new List<string>();
                subjectIds.Add($"а: {item.PatentownerIds}");

                foreach (var li in item.LicenceItems)
                {
                    subjectIds.Add($"б: {li.LicenseeId}");
                }

                return subjectIds;
            case "DateRegistration":
                var dateRegs = new List<string>();
                dateRegs.Add($"");

                foreach (var li in item.LicenceItems)
                {
                    dateRegs.Add($"{li.DateRegistration.ToString("dd.MM.yyyy")}");
                }

                return dateRegs;
            case "NumberRegistration":
                var numRegs = new List<string>();
                numRegs.Add($"");

                foreach (var li in item.LicenceItems)
                {
                    numRegs.Add($"{li.NumberLicence}");
                }

                return numRegs;
            case "Variety":
                return new List<string> { item.VarietyName };
            case "TypeLicence":
                var numRegs2 = new List<string>();
                numRegs2.Add($"");

                foreach (var li in item.LicenceItems)
                {
                    numRegs2.Add($"{li.TypeLicence}");
                }

                return numRegs2;
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
    protected override CellFormatting GetCellFormatting(Licence item, string columnName, string cellText)
    {
        var formatting = new CellFormatting();

        if (columnName == "SubjectIds")
        {
            // Название сорта - подчеркнутое
            formatting.LineFormatting[0] = new CellLineFormatting { IsBold = true };
            // Селекционный номер - курсив
            formatting.LineFormatting[1] = new CellLineFormatting {  };
            formatting.FontSize = FontSizeSmall;
        }
        //else if (columnName == "Applicant")
        //{
        //    // Данные о заявителе/авторе - мелкий шрифт
        //    formatting.FontSize = FontSizeSmall;
        //}

        return formatting;
    }
}