using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfHelperLibrary2
{
    /// <summary>
    /// 文本提取帮助类
    /// </summary>
    public static class TextExtractHelper
    {
        public static void Test(string inputFilename)
        {
            using (var reader = new PdfReader(inputFilename))
            {
                var sb = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    var text = PdfTextExtractor.GetTextFromPage(reader, i);
                    sb.AppendLine(text);
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
}
