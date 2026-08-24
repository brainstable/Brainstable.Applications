using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Brainstable.Applications.Services;

public sealed class WordDocumentMerger
{
    public void MergeDirectory(
        string directoryPath,
        string outputPath,
        string searchPattern = "*.docx")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directoryPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException(directoryPath);

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

    private static void AddDocument(
        MainDocumentPart destinationMainPart,
        Body destinationBody,
        string sourcePath)
    {
        var altChunkPart = destinationMainPart.AddAlternativeFormatImportPart(
            AlternativeFormatImportPartType.WordprocessingML);

        using (var sourceStream = File.OpenRead(sourcePath))
        {
            altChunkPart.FeedData(sourceStream);
        }

        var altChunk = new AltChunk();

        altChunk.Id = destinationMainPart.GetIdOfPart(
            altChunkPart);

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