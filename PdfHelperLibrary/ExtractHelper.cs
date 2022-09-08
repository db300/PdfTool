using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.IO;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 提取帮助类
    /// </summary>
    public class ExtractHelper
    {
        /// <summary>
        /// 提取PDF
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="pageFrom"></param>
        /// <param name="pageTo"></param>
        public static void ExtractPdf(string inputPdfFileName, int pageFrom, int pageTo)
        {
            var inputDocument = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.Import);
            var pageCount = inputDocument.PageCount;
            var path = Path.GetDirectoryName(inputPdfFileName);
            var fileName = Path.GetFileNameWithoutExtension(inputPdfFileName);
            var prefixFileName = Path.Combine(path, fileName);
            var maxPageTo = Math.Min(pageTo, pageCount);
            // Create new document
            var outputDocument = new PdfDocument { Version = inputDocument.Version };
            outputDocument.Info.Title = $"Page {pageFrom} to {maxPageTo} of {inputDocument.Info.Title}";
            outputDocument.Info.Creator = inputDocument.Info.Creator;
            // Add the page and save it
            for (var i = pageFrom - 1; i < maxPageTo; i++) outputDocument.AddPage(inputDocument.Pages[i]);
            outputDocument.Save($"{prefixFileName} - Page {pageFrom} to {maxPageTo}.pdf");
        }
    }
}
