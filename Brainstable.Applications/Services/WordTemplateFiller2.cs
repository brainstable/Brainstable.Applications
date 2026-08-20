using Brainstable.Applications.Models;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using System.Security.Cryptography;

namespace Brainstable.Applications.Services;

public class WordTemplateFiller2
{
    private const string FontName = "Times New Roman";

    // Проценты ширины колонок
    private const string Column1Width = "12";  // Номер заявки
    private const string Column2Width = "28";  // Заявитель/Автор
    private const string Column3Width = "40";  // Название/Селекционный номер
    private const string Column4Width = "20";  // Дата регистрации

    // Размеры шрифтов
    private const string FontSizeNormal = "24";    // 12pt
    private const string FontSizeSmall = "20";     // 10pt

    // Отступы после заголовков
    private const string AfterHeaderBlock = "120";
    private const string AfterHeaderKind = "0";
    private const string AfterHeaderLatine = "240";

    public void FillWordTemplate(string templatePath, string outputPath, List<AcceptedApplication> applications)
    {
        // Копируем шаблон в выходной файл
        File.Copy(templatePath, outputPath, true);

        // Открываем документ для редактирования
        using var wordDocument = WordprocessingDocument.Open(outputPath, true);
        var mainPart = wordDocument.MainDocumentPart;
        var body = mainPart!.Document.Body;

        var groupedByBlock = applications
            .GroupBy(a => new { a.BlockName, a.BlockOrderNumber })
            .OrderBy(g => g.Key.BlockOrderNumber);

        foreach (var blockGroup in groupedByBlock)
        {
            body!.AppendChild(CreateCultureHeader(blockGroup.Key.BlockName, isBold: true, isUnderline: true, afterValue: new StringValue(AfterHeaderBlock)));

            var groupedByKind = blockGroup
                .GroupBy(a => new { a.KindName, a.KindLatineName })
                .OrderBy(g => g.Key.KindName);

            foreach (var kindGroup in groupedByKind)
            {
                body.AppendChild(CreateCultureHeader(kindGroup.Key.KindName, isBold: true));
                body.AppendChild(CreateCultureHeader(kindGroup.Key.KindLatineName, isBold: true, isItalic: true, isUnderline: true, afterValue: new StringValue(AfterHeaderLatine)));
                body.AppendChild(CreateDataTable(kindGroup.OrderBy(x => x.VarietyName).ToList()));
                body.AppendChild(CreateEmptyParagraph());
            }
        }
    }

    private Paragraph CreateCultureHeader(string text, bool isBold = false, bool isItalic = false, bool isUnderline = false, StringValue? afterValue = null)
    {
        var paragraph = new Paragraph();
        paragraph.ParagraphProperties = new ParagraphProperties();

        var spacing = new SpacingBetweenLines();
        if (afterValue is not null)
            spacing.After = afterValue;
        else
        {
            spacing.After = "0";
        }
        paragraph.ParagraphProperties.AppendChild(spacing);

        var run = new Run();
        var textElement = new Text();
        textElement.Text = text;
        run.AppendChild(textElement);

        var runProps = new RunProperties();

        // Устанавливаем шрифт Times New Roman
        var runFonts = new RunFonts();
        runFonts.Ascii = FontName;
        runFonts.HighAnsi = FontName;
        runFonts.ComplexScript = FontName;
        runProps.AppendChild(runFonts);

        var fontSize = new FontSize();
        fontSize.Val = FontSizeNormal;
        runProps.AppendChild(fontSize);

        if (isBold)
        {
            Bold bold = new Bold();
            runProps.AppendChild(bold);
        }

        if (isUnderline)
        {
            Underline underline = new Underline();
            underline.Val = UnderlineValues.Single;
            runProps.AppendChild(underline);
        }

        if (isItalic)
        {
            Italic italic = new Italic();
            runProps.AppendChild(italic);
        }

        run.RunProperties = runProps;
        paragraph.AppendChild(run);

        return paragraph;
    }

    private Table CreateDataTable(List<AcceptedApplication> applications)
    {
        var table = new Table();

        // Настройки таблицы - без границ
        var tableProperties = new TableProperties();
        tableProperties.TableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

        // Убираем все границы
        var borders = new TableBorders();
        borders.TopBorder = new TopBorder() { Val = BorderValues.None };
        borders.BottomBorder = new BottomBorder() { Val = BorderValues.None };
        borders.LeftBorder = new LeftBorder() { Val = BorderValues.None };
        borders.RightBorder = new RightBorder() { Val = BorderValues.None };
        borders.InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.None };
        borders.InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.None };
        tableProperties.AppendChild(borders);

        // Добавляем настройки ширины колонок (в процентах от общей ширины)
        var tableGrid = new TableGrid();

        // Колонка 1: Номер заявки
        var gridColumn1 = new GridColumn();
        gridColumn1.Width = Column1Width;
        tableGrid.AppendChild(gridColumn1);

        // Колонка 2: Заявитель/Автор
        var gridColumn2 = new GridColumn();
        gridColumn2.Width = Column2Width;
        tableGrid.AppendChild(gridColumn2);

        // Колонка 3: Название/Селекционный номер
        var gridColumn3 = new GridColumn();
        gridColumn3.Width = Column3Width;
        tableGrid.AppendChild(gridColumn3);

        // Колонка 4: Дата регистрации
        var gridColumn4 = new GridColumn();
        gridColumn4.Width = Column4Width;
        tableGrid.AppendChild(gridColumn4);

        tableProperties.AppendChild(tableGrid);
        table.AppendChild(tableProperties);

        // Добавляем строки данных (без заголовка)
        foreach (var app in applications)
        {
            TableRow row = CreateDataRow(app);
            table.AppendChild(row);
        }

        return table;
    }

    private TableRow CreateDataRow(AcceptedApplication app)
    {
        var row = new TableRow();

        // Колонка 1: Номер заявки
        row.AppendChild(CreateDataCell(app.VarietyId.ToString(), Column1Width, fontSize: FontSizeNormal));

        // Колонка 2: Заявитель и автор (размер шрифта 10)
        var applicantAndAuthor = $"а: {app.ApplicatorIds}";
        if (!string.IsNullOrEmpty(app.AutorIds))
        {
            applicantAndAuthor += $"\nб: {app.AutorIds}";
        }
        row.AppendChild(CreateDataCell(applicantAndAuthor, Column2Width, fontSize: FontSizeSmall));

        // Колонка 3: Название сорта и селекционный номер (название сорта подчеркнуто, селекционный номер курсив)
        var varietyInfo = $"{app.VarietyName}\n{app.SelectionNumber}";
        row.AppendChild(CreateDataCell(varietyInfo, Column3Width, fontSize: FontSizeNormal, underlineFirstLine: true, italicSecondLine: true));

        // Колонка 4: Дата регистрации
        var dateStr = app.DateAdguardRegistration?.ToString("dd.MM.yyyy") ?? "";
        row.AppendChild(CreateDataCell(dateStr, Column4Width, fontSize: FontSizeNormal));

        return row;
    }

    private TableCell CreateDataCell(string text, string width = "auto", string fontSize = FontSizeNormal, bool underlineFirstLine = false, bool italicSecondLine = false)
    {
        var cell = new TableCell();

        var cellProperties = new TableCellProperties();
        cellProperties.TableCellWidth = new TableCellWidth() { Width = width, Type = TableWidthUnitValues.Pct };

        // Убираем границы ячейки
        var topBorder = new TopBorder();
        topBorder.Val = BorderValues.None;
        cellProperties.AppendChild(topBorder);

        var bottomBorder = new BottomBorder();
        bottomBorder.Val = BorderValues.None;
        cellProperties.AppendChild(bottomBorder);

        var leftBorder = new LeftBorder();
        leftBorder.Val = BorderValues.None;
        cellProperties.AppendChild(leftBorder);

        var rightBorder = new RightBorder();
        rightBorder.Val = BorderValues.None;
        cellProperties.AppendChild(rightBorder);

        cell.AppendChild(cellProperties);

        var paragraph = new Paragraph();
        paragraph.ParagraphProperties = new ParagraphProperties();

        var justification = new Justification();
        justification.Val = JustificationValues.Left;
        paragraph.ParagraphProperties.AppendChild(justification);

        var spacing = new SpacingBetweenLines();
        spacing.After = "200";
        spacing.Line = "240";
        spacing.LineRule = LineSpacingRuleValues.Auto;
        paragraph.ParagraphProperties.AppendChild(spacing);

        // Разбиваем текст на строки
        var lines = text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var run = new Run();
            var textElement = new Text();
            textElement.Text = lines[i];
            run.AppendChild(textElement);

            var runProps = new RunProperties();

            // Устанавливаем шрифт Times New Roman
            var runFonts = new RunFonts();
            runFonts.Ascii = FontName;
            runFonts.HighAnsi = FontName;
            runFonts.ComplexScript = FontName;
            runProps.AppendChild(runFonts);

            // Устанавливаем размер шрифта
            var fontSizeElement = new FontSize();
            fontSizeElement.Val = fontSize;
            runProps.AppendChild(fontSizeElement);

            // Подчеркивание для первой строки (название сорта) в 3-й колонке
            if (underlineFirstLine && i == 0)
            {
                Underline underline = new Underline();
                underline.Val = UnderlineValues.Single;
                runProps.AppendChild(underline);
            }

            // Курсив для второй строки (селекционный номер) в 3-й колонке
            if (italicSecondLine && i == 1)
            {
                Italic italic = new Italic();
                runProps.AppendChild(italic);
            }

            run.RunProperties = runProps;

            paragraph.AppendChild(run);

            // Добавляем перенос строки после каждой строки кроме последней
            if (i < lines.Length - 1)
            {
                var breakElement = new Break();
                paragraph.AppendChild(breakElement);
            }
        }

        cell.AppendChild(paragraph);
        return cell;
    }

    private Paragraph CreateEmptyParagraph()
    {
        var paragraph = new Paragraph();
        paragraph.ParagraphProperties = new ParagraphProperties();

        var spacing = new SpacingBetweenLines();
        spacing.After = "0";
        spacing.Before = "0";
        paragraph.ParagraphProperties.AppendChild(spacing);

        var run = new Run();
        var textElement = new Text();
        textElement.Text = "";
        run.AppendChild(textElement);
        paragraph.AppendChild(run);

        return paragraph;
    }
}