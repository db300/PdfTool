using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfTool
{
    internal static class Config
    {
        internal static string OutputDir
        {
            get
            {
                var dir = $@"{Application.StartupPath}\output\";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return dir;
            }
        }
    }
}
