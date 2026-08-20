using Brainstable.Applications.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brainstable.Applications.Services;

public class WordService
{
    public void AcceptedBuild(List<AcceptedApplication> list)
    {
        var doc = new Openize.Words.Document();
        System.Console.WriteLine("Word Document with default template initialized");

        // Initialize the body with the new document
        var body = new Openize.Words.Body(doc);
        System.Console.WriteLine("Body of the Word Document initialized");

        // Get all table styles
        var tableStyles = doc.GetElementStyles().TableStyles;
        System.Console.WriteLine("Table styles loaded");

        // Create Headings Paragraph and append to the body.
        foreach (var tableStyle in tableStyles)
        {
            var table = new Openize.Words.IElements.Table(5, 3);
            table.Style = tableStyle;

            table.Column.Width = 2500;

            var rowNumber = 0;
            var columnNumber = 0;

            var para = new Openize.Words.IElements.Paragraph();
            para.Style = Openize.Words.IElements.Headings.Heading1;
            para.AddRun(new Openize.Words.IElements.Run
            {
                Text = $"Table With Style '{tableStyle}' : "
            });

            body.AppendChild(para);

            foreach (var row in table.Rows)
            {
                rowNumber++;
                foreach (var cell in row.Cells)
                {
                    columnNumber++;
                    para = new Openize.Words.IElements.Paragraph();
                    para.AddRun(new Openize.Words.IElements.Run
                    {
                        Text = $"Row {rowNumber} Column {columnNumber}"
                    });
                    cell.Paragraphs.Add(para);
                }
                columnNumber = 0;
            }
            body.AppendChild(table);
            System.Console.WriteLine($"Table with style {tableStyle} created and appended");
        }

        // Save the newly created Word Document.
        doc.Save($"WordTables.docx");
    }

}