using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 页面旋转帮助类
    /// </summary>
    public static class PageRotateHelper
    {
        public static string RotatePdf(string inputFilePath, out string outputFilePath, int rotationAngle)
        {
            try
            {
                // 打开现有的 PDF 文档
                var document = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Modify);

                // 遍历每一页并设置旋转角度
                foreach (var page in document.Pages)
                {
                    page.Rotate += rotationAngle;
                }

                // 保存修改后的文档
                outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath), $"{Path.GetFileNameWithoutExtension(inputFilePath)} - {rotationAngle}.pdf");
                document.Save(outputFilePath);
                return "";
            }
            catch (Exception ex)
            {
                outputFilePath = "";
                return ex.Message;
            }
        }

        public static string RotatePdf(string inputFilePath, out string outputFilePath, int rotationAngle, List<int> pageNums)
        {
            try
            {
                // 打开现有的 PDF 文档
                var document = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Modify);

                var pageCount = document.PageCount;
                var validPageNums = pageNums.Where(a => a > 0 && a <= pageCount).ToList();
                validPageNums.Sort();

                foreach (var pageNum in validPageNums)
                {
                    document.Pages[pageNum - 1].Rotate += rotationAngle;
                }

                // 保存修改后的文档
                outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath), $"{Path.GetFileNameWithoutExtension(inputFilePath)} - {rotationAngle}.pdf");
                document.Save(outputFilePath);
                return "";
            }
            catch (Exception ex)
            {
                outputFilePath = "";
                return ex.Message;
            }
        }

        public static string RotatePdf(string inputFilePath, string outputFilePath, Dictionary<int, int> pageRotateDict)
        {
            try
            {
                // 打开现有的 PDF 文档
                var document = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Modify);

                var pageCount = document.PageCount;

                foreach (var item in pageRotateDict)
                {
                    var pageNum = item.Key;
                    var rotateAngle = item.Value * 90;
                    if (pageNum < 0 || pageNum >= pageCount) continue;
                    document.Pages[pageNum].Rotate += rotateAngle;
                }

                // 保存修改后的文档
                document.Save(outputFilePath);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
