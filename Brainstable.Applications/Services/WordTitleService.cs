using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

/// <summary>
/// Сервис создания Word-документов из шаблонов.
/// 
/// Поддерживает:
/// - основной документ;
/// - таблицы;
/// - header;
/// - footer;
/// - placeholder, разбитый Word на несколько Run;
/// - сохранение форматирования существующих Run.
/// </summary>
public sealed class WordTitleService
{
    /// <summary>
    /// Создаёт новый Word-документ из шаблона.
    /// </summary>
    public void Create(string templatePath, string outputPath, IReadOnlyDictionary<string, string> replacements)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templatePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        if (!File.Exists(templatePath))
            throw new FileNotFoundException(
                "Word template was not found.",
                templatePath);

        if (replacements.Count == 0)
            throw new ArgumentException("At least one replacement must be specified.", nameof(replacements));

        var outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));

        if (!string.IsNullOrWhiteSpace(outputDirectory)) Directory.CreateDirectory(outputDirectory);

        File.Copy(templatePath, outputPath, overwrite: true);

        using var document = WordprocessingDocument.Open(outputPath, isEditable: true);

        ProcessMainDocument(document, replacements);
        ProcessHeaders(document, replacements);
        ProcessFooters(document, replacements);

        document.MainDocumentPart?.Document.Save();
    }

    /// <summary>
    /// Удобный overload для твоих трёх placeholder.
    /// </summary>
    public void Create(string templatePath, string outputPath, int nYear, int num, int year)
    {
        var replacements = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["[n_year]"] = nYear.ToString(),
            ["[num]"] = num.ToString(),
            ["[year]"] = year.ToString()
        };

        Create(templatePath, outputPath, replacements);
    }

    private static void ProcessMainDocument(WordprocessingDocument document, IReadOnlyDictionary<string, string> replacements)
    {
        var mainPart = document.MainDocumentPart;

        if (mainPart?.Document.Body is null)
            return;

        ProcessRootElement(mainPart.Document.Body,replacements);
    }

    private static void ProcessHeaders(WordprocessingDocument document, IReadOnlyDictionary<string, string> replacements)
    {
        var mainPart = document.MainDocumentPart;

        if (mainPart is null)
            return;

        foreach (var headerPart in mainPart.HeaderParts)
        {
            if (headerPart.Header is null)
                continue;

            ProcessRootElement(headerPart.Header, replacements);

            headerPart.Header.Save();
        }
    }

    private static void ProcessFooters(WordprocessingDocument document, IReadOnlyDictionary<string, string> replacements)
    {
        var mainPart = document.MainDocumentPart;

        if (mainPart is null)
            return;

        foreach (var footerPart in mainPart.FooterParts)
        {
            if (footerPart.Footer is null)
                continue;

            ProcessRootElement(footerPart.Footer, replacements);

            footerPart.Footer.Save();
        }
    }

    /// <summary>
    /// Обрабатывает все Paragraph внутри указанного корневого элемента.
    ///
    /// Это автоматически покрывает:
    /// - обычный текст;
    /// - таблицы;
    /// - ячейки таблиц;
    /// - вложенные таблицы;
    /// - header;
    /// - footer.
    /// </summary>
    private static void ProcessRootElement(OpenXmlElement root, IReadOnlyDictionary<string, string> replacements)
    {
        foreach (var paragraph in root.Descendants<Paragraph>())
        {
            ReplaceInParagraph(paragraph, replacements);
        }
    }

    /// <summary>
    /// Выполняет замену placeholder внутри одного Paragraph.
    ///
    /// Placeholder может быть разбит между несколькими Run/Text.
    /// Например:
    ///
    /// Run 1 -> "[n_"
    /// Run 2 -> "ye"
    /// Run 3 -> "ar]"
    ///
    /// После обработки:
    ///
    /// Run 1 -> "2"
    /// Run 2 -> ""
    /// Run 3 -> ""
    ///
    /// Форматирование Run 1 сохраняется.
    /// </summary>
    private static void ReplaceInParagraph(Paragraph paragraph, IReadOnlyDictionary<string, string> replacements)
    {
        var textElements = paragraph
            .Descendants<Text>()
            .ToList();

        if (textElements.Count == 0)
            return;

        var text = string.Concat(textElements.Select(x => x.Text));

        if (string.IsNullOrEmpty(text))
            return;

        var replaced = ReplaceAll(text, replacements);

        if (string.Equals(
                text,
                replaced,
                StringComparison.Ordinal))
        {
            return;
        }

        /*
         * Важно:
         *
         * Мы не удаляем Run.
         * Мы не создаём новый Run.
         * Мы только меняем Text.
         *
         * Поэтому rPr существующих Run остаётся нетронутым.
         */

        textElements[0].Text = replaced;

        for (var i = 1; i < textElements.Count; i++)
        {
            textElements[i].Text = string.Empty;
        }
    }

    private static string ReplaceAll(string text, IReadOnlyDictionary<string, string> replacements)
    {
        foreach (var replacement in replacements)
        {
            text = text.Replace(replacement.Key, replacement.Value, StringComparison.Ordinal);
        }

        return text;
    }
}