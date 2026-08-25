using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

public sealed class WordPatentService
{
    /// <summary>
    /// Создаёт новый Word-документ из шаблона
    /// и заменяет placeholder'ы значениями из словаря.
    ///
    /// Поддерживает:
    /// - основной документ;
    /// - таблицы;
    /// - ячейки таблиц;
    /// - вложенные таблицы;
    /// - header;
    /// - footer;
    /// - placeholder, разбитый между несколькими Run;
    /// - многострочные значения;
    /// - сохранение форматирования первого Run placeholder'а.
    /// </summary>
    public void Create(
        string templatePath,
        string outputPath,
        IReadOnlyDictionary<string, string> replacements)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templatePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        ArgumentNullException.ThrowIfNull(replacements);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException(
                "Word template was not found.",
                templatePath);
        }

        if (replacements.Count == 0)
        {
            throw new ArgumentException(
                "At least one replacement must be specified.",
                nameof(replacements));
        }

        var outputDirectory = Path.GetDirectoryName(
            Path.GetFullPath(outputPath));

        if (!string.IsNullOrWhiteSpace(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        File.Copy(
            templatePath,
            outputPath,
            overwrite: true);

        using var document = WordprocessingDocument.Open(
            outputPath,
            isEditable: true);

        ProcessMainDocument(
            document,
            replacements);

        ProcessHeaders(
            document,
            replacements);

        ProcessFooters(
            document,
            replacements);

        document.MainDocumentPart?
            .Document
            .Save();
    }

    private static void ProcessMainDocument(
        WordprocessingDocument document,
        IReadOnlyDictionary<string, string> replacements)
    {
        var mainPart = document.MainDocumentPart;

        if (mainPart?.Document.Body is null)
        {
            return;
        }

        ProcessRootElement(
            mainPart.Document.Body,
            replacements);
    }

    private static void ProcessHeaders(
        WordprocessingDocument document,
        IReadOnlyDictionary<string, string> replacements)
    {
        var mainPart = document.MainDocumentPart;

        if (mainPart is null)
        {
            return;
        }

        foreach (var headerPart in mainPart.HeaderParts)
        {
            if (headerPart.Header is null)
            {
                continue;
            }

            ProcessRootElement(
                headerPart.Header,
                replacements);

            headerPart.Header.Save();
        }
    }

    private static void ProcessFooters(
        WordprocessingDocument document,
        IReadOnlyDictionary<string, string> replacements)
    {
        var mainPart = document.MainDocumentPart;

        if (mainPart is null)
        {
            return;
        }

        foreach (var footerPart in mainPart.FooterParts)
        {
            if (footerPart.Footer is null)
            {
                continue;
            }

            ProcessRootElement(
                footerPart.Footer,
                replacements);

            footerPart.Footer.Save();
        }
    }

    /// <summary>
    /// Обрабатывает все Paragraph внутри элемента.
    ///
    /// Descendants&lt;Paragraph&gt; автоматически позволяет
    /// обрабатывать текст:
    ///
    /// - обычных абзацев;
    /// - таблиц;
    /// - ячеек таблиц;
    /// - вложенных таблиц;
    /// - header;
    /// - footer.
    /// </summary>
    private static void ProcessRootElement(
        OpenXmlElement root,
        IReadOnlyDictionary<string, string> replacements)
    {
        foreach (var paragraph in root.Descendants<Paragraph>())
        {
            ReplaceInParagraph(
                paragraph,
                replacements);
        }
    }

    /// <summary>
    /// Заменяет все placeholder'ы внутри одного Paragraph.
    ///
    /// Поддерживает ситуацию, когда Word разбил placeholder
    /// между несколькими Run.
    ///
    /// Например:
    ///
    /// Run 1 -> "[pat"
    /// Run 2 -> "ent_"
    /// Run 3 -> "owner]"
    ///
    /// Также поддерживаются многострочные значения:
    ///
    /// [patent_owner]
    ///
    /// "ООО Агрофирма
    /// Иванов Иван
    /// Петров Петр"
    ///
    /// Переносы превращаются в Word Break (w:br),
    /// поэтому текст остаётся внутри той же ячейки таблицы.
    /// </summary>
    private static void ReplaceInParagraph(
        Paragraph paragraph,
        IReadOnlyDictionary<string, string> replacements)
    {
        var textElements = paragraph
            .Descendants<Text>()
            .ToList();

        if (textElements.Count == 0)
        {
            return;
        }

        var originalText = string.Concat(
            textElements.Select(x => x.Text));

        if (string.IsNullOrEmpty(originalText))
        {
            return;
        }

        var replacedText = ReplaceAll(
            originalText,
            replacements);

        if (string.Equals(
                originalText,
                replacedText,
                StringComparison.Ordinal))
        {
            return;
        }

        /*
         * Находим Run, содержащий первый Text.
         *
         * Именно его форматирование будет использовано
         * для вставленного значения.
         */
        var firstText = textElements[0];

        var firstRun = firstText
            .Ancestors<Run>()
            .FirstOrDefault();

        if (firstRun is null)
        {
            firstText.Text = replacedText;
            return;
        }

        /*
         * Сохраняем форматирование первого Run.
         *
         * Например:
         * - шрифт;
         * - размер;
         * - жирность;
         * - курсив;
         * - подчёркивание;
         * - цвет;
         * - и т.д.
         */
        var runProperties = firstRun.RunProperties?
            .CloneNode(true);

        /*
         * Очищаем Text всех Run, которые участвовали
         * в исходном тексте Paragraph.
         */
        foreach (var textElement in textElements)
        {
            textElement.Text = string.Empty;
        }

        /*
         * Удаляем содержимое первого Run.
         *
         * RunProperties после этого восстановим.
         */
        firstRun.RemoveAllChildren();

        if (runProperties is not null)
        {
            firstRun.AppendChild(runProperties);
        }

        /*
         * Добавляем новый текст.
         *
         * \r\n, \n и \r превращаются в <w:br/>.
         */
        AppendTextWithLineBreaks(
            firstRun,
            replacedText);
    }

    /// <summary>
    /// Выполняет все замены.
    /// </summary>
    private static string ReplaceAll(
        string text,
        IReadOnlyDictionary<string, string> replacements)
    {
        foreach (var replacement in replacements)
        {
            if (string.IsNullOrEmpty(replacement.Key))
            {
                continue;
            }

            text = text.Replace(
                replacement.Key,
                replacement.Value ?? string.Empty,
                StringComparison.Ordinal);
        }

        return text;
    }

    /// <summary>
    /// Добавляет текст в Run.
    ///
    /// Переносы:
    ///
    /// Environment.NewLine
    /// \r\n
    /// \n
    /// \r
    ///
    /// превращаются в:
    ///
    /// &lt;w:br/&gt;
    ///
    /// Это именно перенос строки внутри текущего
    /// Paragraph / TableCell, а не новый Paragraph
    /// и не новая строка таблицы.
    /// </summary>
    private static void AppendTextWithLineBreaks(
        Run run,
        string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var lines = value
            .Replace(
                "\r\n",
                "\n",
                StringComparison.Ordinal)
            .Replace(
                '\r',
                '\n')
            .Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            if (i > 0)
            {
                run.AppendChild(
                    new Break());
            }

            if (lines[i].Length == 0)
            {
                continue;
            }

            run.AppendChild(
                new Text(lines[i])
                {
                    Space =
                        SpaceProcessingModeValues.Preserve
                });
        }
    }
}