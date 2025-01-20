using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfTool
{
    internal class Common
    {
    }

    internal interface IPdfHandler
    {
        void OpenPdfs(List<string> files);
    }
}
