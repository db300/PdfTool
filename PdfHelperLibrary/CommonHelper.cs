using PdfSharp.Pdf.IO;
using System;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public static class CommonHelper
    {
        public static int GetPageCount(string inputPdfFileName)
        {
            try
            {
                var document = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.Import);
                return document.PageCount;
            }
            catch (PdfReaderException ex)
            {
                throw new PdfReaderException($"{inputPdfFileName}, {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"{inputPdfFileName}, {ex.Message}", ex);
            }
        }
    }
}
