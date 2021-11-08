using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var path0 = @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\证书\";
            var path1 = @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\更名说明\";
            var path2 = @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\合并\";
            var fileList0 = Directory.GetFiles(path0).ToList();
            var fileList1 = Directory.GetFiles(path1).ToList();
            foreach(var file in fileList0)
            {
                var ext = Path.GetExtension(file).ToLower();
                var fileName = Path.GetFileNameWithoutExtension(file);
                var fileName2 = fileList1.FirstOrDefault(a => a.Contains(fileName));
                var outFileName = $"{path2}{fileName}.pdf";
                switch (ext)
                {
                    case ".pdf":
                        PdfHelperLibrary.Builder.InsertImage2Pdf(file, new List<string> { fileName2 }, outFileName);
                        break;
                    default:
                        PdfHelperLibrary.Builder.Image2Pdf(new List<string> { file, fileName2 }, outFileName);
                        break;
                }
            }
            /*
            PdfHelperLibrary.Builder.Image2Pdf(new List<string> {
                @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\更名说明\1-2009-F-020560-汉仪菱心体（简）.jpeg",
                @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\证书\1-2009-F-020560-汉仪菱心体（简）.jpg"}, "a.pdf");
            */
        }
    }
}
