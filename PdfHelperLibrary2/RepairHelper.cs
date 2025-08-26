using iTextSharp.text.pdf;
using System;
using System.IO;

namespace PdfHelperLibrary2
{
    /// <summary>
    /// 修复帮助类
    /// </summary>
    public static class RepairHelper
    {
        public static string SaveAs(string inputFilename, out string outputFilename)
        {
            var dir = Path.GetDirectoryName(inputFilename);
            var prefix = Path.GetFileNameWithoutExtension(inputFilename);
            outputFilename = Path.Combine(dir, $"{prefix}-repaired-{DateTime.Now:yyyyMMddHHmmssfff}.pdf");
            try
            {
                // 打开现有的 PDF 文档
                using (var reader = new PdfReader(inputFilename))
                {
                    // 创建一个新的 PDF 文档
                    using (var fs = new FileStream(outputFilename, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (var stamper = new PdfStamper(reader, fs))
                        {
                            // 这里可以对 pdfStamper 进行操作，例如添加水印、修改内容等
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
