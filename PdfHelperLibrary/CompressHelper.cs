using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 压缩帮助类
    /// </summary>
    public static class CompressHelper
    {
        public static string Compress(string inputFileName, string outputFileName)
        {
            try
            {
                using (var document = PdfReader.Open(inputFileName, PdfDocumentOpenMode.Modify))
                {
                    document.Options.CompressContentStreams = true;
                    document.Save(outputFileName);
                }
                return "";
            }
            catch (Exception ex)
            {
                return $"{inputFileName} 压缩失败，原因：{ex.Message}";
            }
        }
    }
}
