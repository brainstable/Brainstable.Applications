using Brainstable.Applications.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

/// <summary>
/// Заполнитель таблицы II.5 «Сводная таблица предложенных названий» (раздел II).
/// Для каждого блока классов выводит жирный заголовок и перечень названий сортов
/// одной строкой, отсортированной по алфавиту.
/// Шаблон: «2.5 Сводная таблица предложенных названий.docx».
/// </summary>
public class BlockClassVarietyNamesWordFiller
{
    private const string FontName = "Times New Roman";

    // Ширины колонок и отступы оставлены от прежней табличной вёрстки; сейчас не используются.
    private const string Column1Width = "12";
    private const string Column2Width = "28";
    private const string Column3Width = "40";
    private const string Column4Width = "20";

    // Размеры шрифтов в half-points: 24 = 12 pt, 20 = 10 pt.
    private const string FontSizeNormal = "24";
    private const string FontSizeSmall = "20";

    private const string AfterHeaderBlock = "100";
    private const string AfterHeaderKind = "0";
    private const string AfterHeaderLatine = "240";

    /// <summary>
    /// Копирует шаблон и дописывает в конец блоки: заголовок блока классов
    /// и строку названий сортов, входящих в блок.
    /// </summary>
    public void FillWordTemplate(string templatePath, string outputPath, List<BlockClassVarietyName> applications)
    {
        // Копируем шаблон в выходной файл
        File.Copy(templatePath, outputPath, true);

        // Открываем документ для редактирования
        using var wordDocument = WordprocessingDocument.Open(outputPath, true);
        var mainPart = wordDocument.MainDocumentPart;
        var body = mainPart!.Document.Body;

        var groupedByBlock = applications
            .GroupBy(a => new { a.BlockClassName, a.BlockClassId })
            .OrderBy(g => g.Key.BlockClassId);

        foreach (var blockGroup in groupedByBlock)
        {
            body!.AppendChild(CreateCultureHeader(blockGroup.Key.BlockClassName, isBold: true, isUnderline: false, afterValue: new StringValue(AfterHeaderBlock)));

            var list = new List<string>();

            foreach (var name in blockGroup)
            {
                list.Add(name.VarietyName);
            }

            var varietyNames = string.Join(", ", list.OrderBy(x => x));
            body.AppendChild(CreateCultureHeader(varietyNames));
            body.AppendChild(CreateEmptyParagraph());
        }
    }

    /// <summary>Создаёт центрированный параграф с заданным форматированием.</summary>
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

        var justification = new Justification();
        justification.Val = JustificationValues.Center;
        runProps.AppendChild(justification);

        run.RunProperties = runProps;
        paragraph.AppendChild(run);

        return paragraph;
    }

    /// <summary>Создаёт пустой параграф-разделитель между блоками.</summary>
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