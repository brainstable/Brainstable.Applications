using Brainstable.Applications.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

/// <summary>
/// Заполнитель таблицы VIII.1 «Список заявителей, авторов и патентообладателей» (раздел VIII).
/// Своя вёрстка: две колонки без заголовков — номер субъекта (жирный, по правому краю)
/// и наименование субъекта (мелкий шрифт).
/// Шаблон: «8. Субъекты.docx».
/// </summary>
public class SubjectNamesWordFiller
{
    private const string FontName = "Times New Roman";

    // Ширина колонок в процентах: номер субъекта и наименование.
    private const string Column1Width = "10";
    private const string Column2Width = "90";

    // Размеры шрифтов в half-points: 18 = 9 pt, 16 = 8 pt.
    private const string FontSizeNormal = "18";
    private const string FontSizeSmall = "16";

    /// <summary>
    /// Копирует шаблон и дописывает в конец таблицу субъектов, отсортированных по номеру.
    /// </summary>
    public void FillWordTemplate(string templatePath, string outputPath, List<SubjectName> subjectNames)
    {
        // Копируем шаблон в выходной файл
        File.Copy(templatePath, outputPath, true);

        // Открываем документ для редактирования
        using var wordDocument = WordprocessingDocument.Open(outputPath, true);
        var mainPart = wordDocument.MainDocumentPart;
        var body = mainPart!.Document.Body;

        body!.AppendChild(CreateDataTable(subjectNames));
    }

    /// <summary>Создаёт таблицу без границ: номер субъекта и его наименование.</summary>
    private Table CreateDataTable(List<SubjectName> applications)
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

        tableProperties.AppendChild(tableGrid);
        table.AppendChild(tableProperties);

        // Добавляем строки данных (без заголовка)
        foreach (var app in applications.OrderBy(x => x.Id))
        {
            TableRow row = CreateDataRow(app);
            table.AppendChild(row);
        }

        return table;
    }

    /// <summary>Создаёт строку таблицы: номер субъекта и наименование.</summary>
    private TableRow CreateDataRow(SubjectName app)
    {
        var row = new TableRow();

        // Колонка 1: Номер заявки
        row.AppendChild(CreateDataCell2(app.Id.ToString(), Column1Width, fontSize: FontSizeNormal, isBold: true));

        // Колонка 2: Заявитель и автор (размер шрифта 10)
        row.AppendChild(CreateDataCell(app.Name, Column2Width, fontSize: FontSizeSmall));

        return row;
    }

    /// <summary>Создаёт ячейку с выравниванием влево (наименование субъекта).</summary>
    private TableCell CreateDataCell(string text, string width = "auto", string fontSize = FontSizeNormal, bool underlineFirstLine = false, 
        bool italicSecondLine = false, bool isBold = false)
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

        //var spacing = new SpacingBetweenLines();
        //spacing.After = "150";
        //spacing.Line = "150";
        //spacing.LineRule = LineSpacingRuleValues.Auto;
        //paragraph.ParagraphProperties.AppendChild(spacing);

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

            if (isBold)
            {
                Bold bold = new Bold();
                runProps.AppendChild(bold);
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
    /// <summary>Создаёт ячейку с выравниванием вправо (номер субъекта).</summary>
    private TableCell CreateDataCell2(string text, string width = "auto", string fontSize = FontSizeNormal, bool underlineFirstLine = false,
    bool italicSecondLine = false, bool isBold = false)
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
        justification.Val = JustificationValues.Right;
        paragraph.ParagraphProperties.AppendChild(justification);

        //var spacing = new SpacingBetweenLines();
        //spacing.After = "150";
        //spacing.Line = "150";
        //spacing.LineRule = LineSpacingRuleValues.Auto;
        //paragraph.ParagraphProperties.AppendChild(spacing);

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

            if (isBold)
            {
                Bold bold = new Bold();
                runProps.AppendChild(bold);
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
}