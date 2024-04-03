using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 打印帮助类
    /// </summary>
    public static class PrintHelper
    {
        public static string Print(string inputPdfFileName)
        {
            try
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        CreateNoWindow = true,
                        Verb = "print",
                        FileName = inputPdfFileName //put the correct path here
                    }
                };
                p.Start();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
