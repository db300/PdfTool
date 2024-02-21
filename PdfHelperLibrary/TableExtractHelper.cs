using System.Collections.Generic;
using System.Linq;
using Tabula;
using Tabula.Extractors;
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
            var extractTables = new List<PdfExtractTable>();
            using (var document = PdfDocument.Open(inputPdfFileName, new ParsingOptions() { ClipPaths = true }))
            {
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
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(pdfExtractTables);
#endif
                    extractTables.AddRange(pdfExtractTables);
                }
            }
            return (true, "", extractTables);
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
