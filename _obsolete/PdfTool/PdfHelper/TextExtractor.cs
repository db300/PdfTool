using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.IO;

namespace PdfTool.PdfHelper
{
    /// <summary>
    /// 文本提取器
    /// </summary>
    internal class TextExtractor
    {
        internal static string GetText(string fileName)
        {
            using (var doc = PdfReader.Open(fileName, PdfDocumentOpenMode.ReadOnly))
            {
                var sb = new StringBuilder();
                foreach (var page in doc.Pages)
                {
                    foreach (var content in page.Contents)
                    {
                        System.Diagnostics.Debug.WriteLine(content);
                    }
                }

                return sb.ToString();
            }
        }
    }
}
