using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeHelperLibrary
{
    public static class WordHelper
    {
        public static void CreateFile(string fileName, List<string> list)
        {
            var doc = new XWPFDocument();
            foreach (var s in list)
            {
                var p = doc.CreateParagraph();
                var r = p.CreateRun();
                r.SetText(s);
            }
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                doc.Write(fs);
                fs.Close();
            }
            doc.Close();
        }
    }
}
