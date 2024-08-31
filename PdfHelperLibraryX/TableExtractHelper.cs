using Tabula.Extractors;
using Tabula;
using UglyToad.PdfPig;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 表格提取帮助类
    /// </summary>
    public static class TableExtractHelper
    {
        public static (bool, string, List<PdfExtractTable>) ExtractTable(string inputPdfFileName)
        {
            using (var document = PdfDocument.Open(inputPdfFileName, new ParsingOptions() { ClipPaths = true }))
            {
                var extractTables = ExtractTable(document);
                return (true, "", extractTables);
            }
        }

        public static (bool, string, List<PdfExtractRow>) ExtractTableRows(string inputPdfFileName)
        {
            using (var document = PdfDocument.Open(inputPdfFileName, new ParsingOptions() { ClipPaths = true }))
            {
                var extractTableRows = ExtractTableRows(document);
                return (true, "", extractTableRows);
            }
        }

        private static List<PdfExtractTable> ExtractTable(PdfDocument document)
        {
            var extractTables = new List<PdfExtractTable>();
            var oe = new ObjectExtractor(document);
            var pageCount = document.NumberOfPages;
            for (var i = 1; i <= pageCount; i++)
            {
                var pageArea = oe.Extract(i);
                var ea = new SpreadsheetExtractionAlgorithm();
                var tables = ea.Extract(pageArea);
                var pdfExtractTables = tables.Select(t => new PdfExtractTable
                {
                    Rows = t.Rows.Select(r => new PdfExtractRow
                    {
                        Cells = r.Select(c => c.GetText()).ToList()
                    }).ToList()
                }).ToList();
                extractTables.AddRange(pdfExtractTables);
            }
            return extractTables;
        }

        private static List<PdfExtractRow> ExtractTableRows(PdfDocument document)
        {
            var rows = new List<PdfExtractRow>();
            var oe = new ObjectExtractor(document);
            var pageCount = document.NumberOfPages;
            for (var i = 1; i <= pageCount; i++)
            {
                var pageArea = oe.Extract(i);
                var ea = new SpreadsheetExtractionAlgorithm();
                var tables = ea.Extract(pageArea);
                tables.ForEach(table =>
                {
                    var rs = table.Rows.Select(r => new PdfExtractRow
                    {
                        Cells = r.Select(c => c.GetText()).ToList()
                    }).ToList();
                    rows.AddRange(rs);
                });
            }
            return rows;
        }
    }

    public class PdfExtractTable
    {
        public List<PdfExtractRow> Rows;
    }

    public class PdfExtractRow
    {
        public List<string> Cells;
    }
}
