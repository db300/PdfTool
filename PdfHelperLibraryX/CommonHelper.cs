using PdfSharpCore.Pdf.IO;

namespace PdfHelperLibraryX
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public static class CommonHelper
    {
        public static int GetPageCount(string inputPdfFileName)
        {
            var document = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.ReadOnly);
            return document.PageCount;
        }
    }
}
