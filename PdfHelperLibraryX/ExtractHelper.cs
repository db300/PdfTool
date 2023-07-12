using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PdfHelperLibraryX
{
    /// <summary>
    /// 提取帮助类
    /// </summary>
    public static class ExtractHelper
    {
        /// <summary>
        /// 提取PDF
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="pageFrom"></param>
        /// <param name="pageTo"></param>
        public static string ExtractPdf(string inputPdfFileName, int pageFrom, int pageTo, out string outputPdfFileName)
        {
            try
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
                outputPdfFileName = $"{prefixFileName} - Page {pageFrom} to {maxPageTo}.pdf";
                outputDocument.Save(outputPdfFileName);
                return "";
            }
            catch (Exception ex)
            {
                outputPdfFileName = "";
                return ex.Message;
            }
        }
    }
}
