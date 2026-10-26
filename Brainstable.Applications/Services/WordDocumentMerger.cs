using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

/// <summary>
/// Склеивает все .docx-файлы каталога в один документ.
///
/// Первый файл берётся за основу, остальные вставляются в конец через механизм AltChunk
/// с разрывом страницы. Итоговый документ окончательно собирается средствами Word при открытии.
/// </summary>
public sealed class WordDocumentMerger
{
    /// <summary>
    /// Объединяет документы каталога в файл <paramref name="outputPath"/>.
    /// Файлы обрабатываются в алфавитном порядке; сам итоговый файл из выборки исключается.
    /// </summary>
    /// <param name="directoryPath">Каталог с исходными .docx.</param>
    /// <param name="outputPath">Путь к итоговому документу.</param>
    /// <param name="searchPattern">Маска отбора файлов, по умолчанию все .docx.</param>
    public void MergeDirectory(
        string directoryPath,
        string outputPath,
        string searchPattern = "*.docx")
    {
        // Проверяем входные параметры и наличие каталога.
        ArgumentException.ThrowIfNullOrWhiteSpace(directoryPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException(directoryPath);

        // Собираем исходные файлы, исключая сам итоговый документ, и сортируем по имени.
        var files = Directory
            .EnumerateFiles(directoryPath, searchPattern)
            .Where(x => !string.Equals(
                Path.GetFullPath(x),
                Path.GetFullPath(outputPath),
                StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (files.Count == 0)
            throw new InvalidOperationException(
                $"No Word files found in '{directoryPath}'.");

        var outputDirectory = Path.GetDirectoryName(
            Path.GetFullPath(outputPath));

        if (!string.IsNullOrWhiteSpace(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        // Удаляем предыдущий результат, чтобы не дописывать документы поверх старого файла.
        if (File.Exists(outputPath))
            File.Delete(outputPath);

        // Берём первый документ как основу.
        File.Copy(files[0], outputPath);

        using var destination = WordprocessingDocument.Open(
            outputPath,
            true);

        var mainPart = destination.MainDocumentPart
                       ?? throw new InvalidOperationException(
                           "Destination document does not contain MainDocumentPart.");

        var body = mainPart.Document.Body
                   ?? throw new InvalidOperationException(
                       "Destination document does not contain Body.");

        // Добавляем остальные документы.
        for (var i = 1; i < files.Count; i++)
        {
            AddDocument(
                mainPart,
                body,
                files[i]);
        }

        mainPart.Document.Save();
    }

    /// <summary>
    /// Вставляет содержимое исходного документа в конец итогового:
    /// сначала разрыв страницы, затем AltChunk с телом файла.
    /// </summary>
    /// <param name="destinationMainPart">Основная часть итогового документа.</param>
    /// <param name="destinationBody">Тело итогового документа.</param>
    /// <param name="sourcePath">Путь к вставляемому .docx.</param>
    private static void AddDocument(
        MainDocumentPart destinationMainPart,
        Body destinationBody,
        string sourcePath)
    {
        // Создаём часть AltChunk и копируем в неё исходный документ.
        var altChunkPart = destinationMainPart.AddAlternativeFormatImportPart(
            AlternativeFormatImportPartType.WordprocessingML);

        using (var sourceStream = File.OpenRead(sourcePath))
        {
            altChunkPart.FeedData(sourceStream);
        }

        var altChunk = new AltChunk();

        altChunk.Id = destinationMainPart.GetIdOfPart(
            altChunkPart);

        // Перед вставляемым разделом ставим разрыв страницы.
        destinationBody.AppendChild(
            new Paragraph(
                new Run(
                    new Break
                    {
                        Type = BreakValues.Page
                    })));

        destinationBody.AppendChild(altChunk);
    }
}
