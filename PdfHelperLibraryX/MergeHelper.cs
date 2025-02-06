using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 合并帮助类
    /// </summary>
    public static class MergeHelper
    {
        public static string MergePdf(List<string> inputPdfFilenameList, bool addBookmarks, out string outputPdfFilename)
        {
            try
            {
                var outputDocument = new PdfDocument();
                foreach (var file in inputPdfFilenameList)
                {
                    var inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                    var pageCount = inputDocument.PageCount;
                    for (var i = 0; i < pageCount; i++)
                    {
                        var page = outputDocument.AddPage(inputDocument.Pages[i]);
                        if (i == 0 && addBookmarks) outputDocument.Outlines.Add(Path.GetFileNameWithoutExtension(file), page);
                    }
                }
                var path = Path.GetDirectoryName(inputPdfFilenameList.First());
                outputPdfFilename = Path.Combine(path, $"MergedFile - {DateTime.Now:yyyyMMddHHmmssfff}.pdf");
                outputDocument.Save(outputPdfFilename);
                return "";
            }
            catch (Exception ex)
            {
                outputPdfFilename = "";
                return $"合并失败，原因：{ex.Message}";
            }
        }
    }
}
