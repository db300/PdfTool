using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spire.Pdf;
using Spire.Pdf.Utilities;

namespace PdfTableExtractor
{
    public static class Common
    {
        private const string EvaluationWarning = "Evaluation Warning : The document was created with Spire.PDF for .NET.";

        public static List<List<string>> Pdf2Table(string fileName, List<int> filterRowList, List<int> filterColList)
        {
            var streamList = SplitPdf(fileName);
            if (!(streamList?.Count > 0)) return null;
            return Pdf2Table(streamList, filterRowList, filterColList);
        }

        internal static List<List<string>> Pdf2Table(Stream stream, List<int> filterRowList, List<int> filterColList)
        {
            var streamList = SplitPdf(stream);
            if (!(streamList?.Count > 0)) return null;
            return Pdf2Table(streamList, filterRowList, filterColList);
        }

        private static List<List<string>> Pdf2Table(List<Stream> streamList, List<int> filterRowList, List<int> filterColList)
        {
            var result = new List<List<string>>();
            var index = 0;
            foreach (var stream in streamList)
            {
                index++;
                System.Diagnostics.Debug.WriteLine($"正在处理第 {index} 页");
                var list = Pdf2Table4OnePage(stream, filterRowList, filterColList);
                if (!(list?.Count > 0)) continue;
                result.AddRange(list);
            }
            return result;
        }

        private static List<List<string>> Pdf2Table4OnePage(Stream stream, List<int> filterRowList, List<int> filterColList)
        {
            try
            {
                var result = new List<List<string>>();
                var document = new PdfDocument(stream);
                var tableExtractor = new Spire.Pdf.Utilities.PdfTableExtractor(document);
                PdfTable[] tableList = null;
                for (var pageIndex = 0; pageIndex < document.Pages.Count; pageIndex++)
                {
                    tableList = tableExtractor.ExtractTable(pageIndex);
                    if (!(tableList?.Length > 0)) continue;
                    foreach (var table in tableList)
                    {
                        var row = table.GetRowCount();
                        var col = table.GetColumnCount();
                        for (var i = 0; i < row; i++)
                        {
                            if (filterRowList.Contains(i)) continue;
                            var cols = new List<string>();
                            for (var j = 0; j < col; j++)
                            {
                                if (filterColList.Contains(j)) continue;
                                try
                                {
                                    var cell = table.GetText(i, j);
                                    cols.Add(cell);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"row: {i}, col: {j}, {ex.Message}");
                                }
                            }
                            result.Add(cols);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// pdf提取文本
        /// </summary>
        internal static List<string> Pdf2String(string fileName)
        {
            try
            {
                var document = new PdfDocument(fileName);
                var sb = new StringBuilder();
                //foreach (PdfPageBase page in document.Pages) sb.Append(page.ExtractText());
                document.Close();

                var ss = sb.ToString().Replace(EvaluationWarning, "").Trim().Split('\n').Select(a => a.Trim()).ToList();
                ss.RemoveAll(string.IsNullOrWhiteSpace);
                return ss;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// pdf提取文本
        /// </summary>
        internal static List<string> Pdf2String(Stream stream)
        {
            try
            {
                var document = new PdfDocument(stream);
                var sb = new StringBuilder();
                //foreach (PdfPageBase page in document.Pages) sb.Append(page.ExtractText());
                document.Close();

                var ss = sb.ToString().Replace(EvaluationWarning, "").Trim().Split('\n').Select(a => a.Trim()).ToList();
                ss.RemoveAll(string.IsNullOrWhiteSpace);
                return ss;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// pdf提取文本
        /// </summary>
        internal static List<string> Pdf2String(byte[] bytes)
        {
            try
            {
                var document = new PdfDocument(bytes);
                var sb = new StringBuilder();
                //foreach (PdfPageBase page in document.Pages) sb.Append(page.ExtractText());
                document.Close();

                var ss = sb.ToString().Replace(EvaluationWarning, "").Trim().Split('\n').Select(a => a.Trim()).ToList();
                ss.RemoveAll(string.IsNullOrWhiteSpace);
                return ss;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        private static List<Stream> SplitPdf(string fileName)
        {
            // Open the file
            var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(fileName, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
            var streamList = new List<Stream>();
            for (int idx = 0; idx < inputDocument.PageCount; idx++)
            {
                // Create new document
                var outputDocument = new PdfSharp.Pdf.PdfDocument
                {
                    Version = inputDocument.Version
                };
                outputDocument.Info.Title = String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[idx]);
                var ms = new MemoryStream();
                outputDocument.Save(ms);
                streamList.Add(ms);
            }
            return streamList;
        }

        private static List<Stream> SplitPdf(Stream stream)
        {
            // Open the file
            var inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(stream, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
            var streamList = new List<Stream>();
            for (int idx = 0; idx < inputDocument.PageCount; idx++)
            {
                // Create new document
                var outputDocument = new PdfSharp.Pdf.PdfDocument
                {
                    Version = inputDocument.Version
                };
                outputDocument.Info.Title = String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
                outputDocument.Info.Creator = inputDocument.Info.Creator;

                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[idx]);
                var ms = new MemoryStream();
                outputDocument.Save(ms);
                streamList.Add(ms);
            }
            return streamList;
        }
    }
}
