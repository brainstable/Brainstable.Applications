using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

public static class WordMerger
{
    /// <summary>
    /// Объединяет все файлы Word (.docx) из указанной директории в один документ
    /// </summary>
    public static int MergeWordDocuments(string directoryPath, string outputPath,
                                         string searchPattern = "*.docx",
                                         bool addPageBreaks = true)
    {
        if (string.IsNullOrEmpty(directoryPath))
            throw new ArgumentException("Путь к директории не может быть пустым", nameof(directoryPath));

        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Директория не найдена: {directoryPath}");

        var wordFiles = Directory.GetFiles(directoryPath, searchPattern)
                                 .OrderBy(f => f)
                                 .ToList();

        if (wordFiles.Count == 0)
            return 0;

        // Открываем первый файл для чтения
        using var firstDoc = WordprocessingDocument.Open(wordFiles[0], false);
        // Создаем новый документ
        using var targetDoc = WordprocessingDocument.Create(outputPath,
            firstDoc.DocumentType);

        // Добавляем основную часть документа
        var mainPart = targetDoc.AddMainDocumentPart();
        mainPart.Document = new Document();

        // Копируем стили из первого документа
        CopyStyles(firstDoc, targetDoc);

        // Создаем тело документа
        var body = new Body();

        // Копируем содержимое первого файла
        var firstBody = firstDoc.MainDocumentPart.Document.Body;
        foreach (var element in firstBody.ChildElements)
        {
            body.AppendChild(element.CloneNode(true));
        }

        // Добавляем остальные файлы
        for (int i = 1; i < wordFiles.Count; i++)
        {
            using var sourceDoc = WordprocessingDocument.Open(wordFiles[i], false);

            // Копируем стили из исходного документа
            CopyStyles(sourceDoc, targetDoc);

            if (addPageBreaks)
            {
                var paragraph = new Paragraph();
                var run = new Run();
                var breakElement = new Break { Type = BreakValues.Page };
                run.AppendChild(breakElement);
                paragraph.AppendChild(run);
                body.AppendChild(paragraph);
            }

            var sourceBody = sourceDoc.MainDocumentPart.Document.Body;
            foreach (var element in sourceBody.ChildElements)
            {
                body.AppendChild(element.CloneNode(true));
            }
        }

        // Устанавливаем тело документа
        mainPart.Document.Body = body;
        mainPart.Document.Save();

        return wordFiles.Count;
    }

    /// <summary>
    /// Копирует стили из исходного документа в целевой
    /// </summary>
    private static void CopyStyles(WordprocessingDocument sourceDoc, WordprocessingDocument targetDoc)
    {
        var sourceStylesPart = sourceDoc.MainDocumentPart.StyleDefinitionsPart;
        if (sourceStylesPart == null)
            return;

        // Если в целевом документе нет части стилей, создаем ее
        var targetStylesPart = targetDoc.MainDocumentPart.StyleDefinitionsPart;
        if (targetStylesPart == null)
        {
            targetStylesPart = targetDoc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            targetStylesPart.Styles = new Styles();
        }

        var sourceStyles = sourceStylesPart.Styles;
        if (sourceStyles == null)
            return;

        var targetStyles = targetStylesPart.Styles;

        // Копируем каждый стиль, если его еще нет в целевом документе
        foreach (var style in sourceStyles.ChildElements.OfType<Style>())
        {
            var styleId = style.StyleId?.Value;
            if (string.IsNullOrEmpty(styleId))
                continue;

            // Проверяем, существует ли уже такой стиль
            var existingStyle = targetStyles.ChildElements
                .OfType<Style>()
                .FirstOrDefault(s => s.StyleId?.Value == styleId);

            if (existingStyle == null)
            {
                // Клонируем стиль и добавляем в целевой документ
                var clonedStyle = (Style)style.CloneNode(true);
                targetStyles.AppendChild(clonedStyle);
            }
        }
    }
}