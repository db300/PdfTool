using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;

namespace PdfHelperLibraryX
{
    /// <summary>
    /// 拆分帮助类
    /// </summary>
    public static class SplitHelper
    {
        public static void SplitPdf(string inputPdfFileName)
        {
            var inputDocument = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.Import);
            var pageCount = inputDocument.PageCount;
            var path = Path.GetDirectoryName(inputPdfFileName);
            var fileName = Path.GetFileNameWithoutExtension(inputPdfFileName);
            var prefixFileName = Path.Combine(path, fileName);
            for (var i = 0; i < pageCount; i++)
            {
                // Create new document
                var outputDocument = new PdfDocument { Version = inputDocument.Version };
                outputDocument.Info.Title = $"Page {i + 1} of {inputDocument.Info.Title}";
                outputDocument.Info.Creator = inputDocument.Info.Creator;
                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[i]);
                outputDocument.Save($"{prefixFileName} - Page {i + 1}.pdf");
            }
        }

        /// <summary>
        /// 拆分PDF(按页数)
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="pageCountPerDoc">拆分后每个文档页数</param>
        public static string SplitPdf(string inputPdfFileName, int pageCountPerDoc)
        {
            try
            {
                var inputDocument = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.Import);
                var pageCount = inputDocument.PageCount;
                var path = Path.GetDirectoryName(inputPdfFileName);
                var fileName = Path.GetFileNameWithoutExtension(inputPdfFileName);
                var prefixFileName = Path.Combine(path, fileName);
                for (var i = 0; i < pageCount;)
                {
                    var subPageCount = Math.Min(pageCountPerDoc, pageCount - i);
                    var pageLabel = subPageCount > 1
                        ? $"{i + 1} - {i + subPageCount}"
                        : $"{i + 1}";
                    // Create new document
                    var outputDocument = new PdfDocument { Version = inputDocument.Version };
                    outputDocument.Info.Title = $"Page {pageLabel} of {inputDocument.Info.Title}";
                    outputDocument.Info.Creator = inputDocument.Info.Creator;
                    // Add the page and save it
                    for (var j = 0; j < subPageCount; j++) outputDocument.AddPage(inputDocument.Pages[i + j]);
                    outputDocument.Save($"{prefixFileName} - Page {pageLabel}.pdf");
                    i += subPageCount;
                }
                return "";
            }
            catch (Exception ex)
            {
                return $"拆分失败，原因：{ex.Message}";
            }
        }
    }
}
