using Brainstable.Applications.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

public class SubjectNamesWordFiller2
{
    private const string FontName = "Times New Roman";

    // Размеры шрифтов
    private const string FontSizeNormal = "16";    // 12pt

    // Отступы после заголовков
    private const string AfterHeaderBlock = "80";

    public void FillWordTemplate(string templatePath, string outputPath, List<SubjectName> applications)
    {
        // Копируем шаблон в выходной файл
        File.Copy(templatePath, outputPath, true);

        // Открываем документ для редактирования
        using var wordDocument = WordprocessingDocument.Open(outputPath, true);
        var mainPart = wordDocument.MainDocumentPart;
        var body = mainPart!.Document.Body;

        foreach (var subjectName in applications.OrderBy(x => x.Id))
        {
            body!.AppendChild(CreateCultureHeader($"{subjectName.Id}\t{subjectName.Name}", isBold: true, isUnderline: false, afterValue: new StringValue(AfterHeaderBlock)));
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

        var justification = new Justification();
        justification.Val = JustificationValues.Center;
        runProps.AppendChild(justification);

        run.RunProperties = runProps;
        paragraph.AppendChild(run);

        return paragraph;
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