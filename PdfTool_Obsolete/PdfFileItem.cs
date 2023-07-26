using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTool
{
    internal class PdfFileItem
    {
        public string SrcFileName { get; set; }
        public string Progress { get; set; }
        public List<string> TextLines { get; set; }
        public string DesFileName { get; set; }
    }
}
