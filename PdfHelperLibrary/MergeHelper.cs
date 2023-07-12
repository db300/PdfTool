using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 合并帮助类
    /// </summary>
    public static class MergeHelper
    {
        public static string MergePdf(List<string> inputPdfFilenameList, out string outputPdfFilename)
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
                        outputDocument.AddPage(inputDocument.Pages[i]);
                    }
                }
                var path = Path.GetDirectoryName(inputPdfFilenameList.First());
                outputPdfFilename = Path.Combine(path, $"MergedFile - {DateTime.Now:yyyyMMddHHmmssfff}.pdf");
                outputDocument.Save(outputPdfFilename);
                Process.Start(outputPdfFilename);
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
