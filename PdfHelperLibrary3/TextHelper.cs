using Spire.Pdf;
using Spire.Pdf.Texts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfHelperLibrary3
{
    public static class TextHelper
    {
        private const string EvaluationWarning = "Evaluation Warning : The document was created with Spire.PDF for .NET.";

        /// <summary>
        /// pdf提取文本
        /// </summary>
        public static List<string> Pdf2String(string fileName)
        {
            try
            {
                var document = new PdfDocument(fileName);
                var sb = new StringBuilder();
                var options = new PdfTextExtractOptions();
                foreach (PdfPageBase page in document.Pages)
                {
                    var textExtractor = new PdfTextExtractor(page);
                    var text = textExtractor.ExtractText(options);
                    sb.Append(text);
                }
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
        public static List<string> Pdf2String(Stream stream)
        {
            try
            {
                var document = new PdfDocument(stream);
                var sb = new StringBuilder();
                var options = new PdfTextExtractOptions();
                foreach (PdfPageBase page in document.Pages)
                {
                    var textExtractor = new PdfTextExtractor(page);
                    var text = textExtractor.ExtractText(options);
                    sb.Append(text);
                }
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
        public static List<string> Pdf2String(byte[] bytes)
        {
            try
            {
                var document = new PdfDocument(bytes);
                var sb = new StringBuilder();
                var options = new PdfTextExtractOptions();
                foreach (PdfPageBase page in document.Pages)
                {
                    var textExtractor = new PdfTextExtractor(page);
                    var text = textExtractor.ExtractText(options);
                    sb.Append(text);
                }
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
    }
}
