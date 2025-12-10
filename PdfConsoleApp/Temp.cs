using System;
using System.IO;
using System.Linq;

namespace PdfConsoleApp
{
    internal class Temp
    {
        internal static void Test()
        {
            Console.WriteLine("Temp Test");

            //测试骑缝章方法
            const string inputFileName = @"骑缝章测试文件.pdf";
            const string stampFileName = @"骑缝章文件.png";
            PdfHelperLibrary.SealHelper.CrossPageSeal(stampFileName, inputFileName, 0.4);
            return;


            //批量检查异常文件
            var dir = @"C:\GitHub\FileDownloader\FileDownloader\bin\Debug\Downloads";
            var files = Directory.GetFiles(dir, "*.pdf", SearchOption.AllDirectories).ToList();
            int errCount = 0, okCount = 0;
            foreach (var file in files)
            {
                try
                {
                    PdfHelperLibrary.CommonHelper.GetPageCount(file);
                    okCount++;
                }
                catch (Exception ex)
                {
                    errCount++;
                    Console.Error.WriteLine(ex.Message);
                }
            }
            Console.Out.WriteLine($"检查完成，共{files.Count}个文件，正常{okCount}个，异常{errCount}个");
            Console.ReadLine();


            /*
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
            */
            /*
            PdfHelperLibrary.Builder.Image2Pdf(new List<string> {
                @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\更名说明\1-2009-F-020560-汉仪菱心体（简）.jpeg",
                @"C:\Users\冷怀晶\Downloads\B端字体著作权登记证书及更名说明（285份）\证书\1-2009-F-020560-汉仪菱心体（简）.jpg"}, "a.pdf");
            */
            /*
            var pathSrc = @"C:\Users\冷怀晶\source\repos\FileDownloader\FileDownloader\bin\Debug\temp";
            var dirs = Directory.GetDirectories(pathSrc);
            foreach (var dir in dirs)
            {
                Console.WriteLine(dir);
                var files = Directory.GetFiles(dir, "*.pdf");
                foreach (var file in files)
                {
                    var s = PdfHelperLibrary.ImagerHelper.ConvertPdfToImage(file, 0, dir);
                    if (string.IsNullOrWhiteSpace(s)) Console.Out.WriteLine($"{file} 转换完成");
                    else Console.Out.WriteLine($"{file} {s}");
                }
            }
            */
        }
    }
}
