using OfficeOpenXml.Drawing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToolConsoleApp
{
    internal class Program
    {
        private const string FileName = @"C:\test\test.xlsx";

        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            ExtractImages(FileName);
            //ExcelHelperLibrary.ImageExtractHelper.ExtractImages(FileName);
            Console.WriteLine("done");
            Console.ReadLine();
        }

        public static void ExtractImages(string excelFilePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var workbook = package.Workbook;
                foreach (var worksheet in workbook.Worksheets)
                {
                    var images = new List<(ExcelPicture picture, int row, int column)>();

                    foreach (var drawing in worksheet.Drawings)
                    {
                        if (drawing is ExcelPicture picture)
                        {
                            var row = picture.From.Row + 1; // EPPlus uses 0-based index
                            var column = picture.From.Column + 1; // EPPlus uses 0-based index
                            images.Add((picture, row, column));
                        }
                    }

                    foreach (var (picture, row, column) in images)
                    {
                        Console.WriteLine($"Image at Row: {row}, Column: {column}");
                        // Save the image to a file or process it as needed
                        var imagePath = Path.Combine("output", $"{worksheet.Name}_Image_{row}_{column}.png");
                        Directory.CreateDirectory("output");
                        File.WriteAllBytes(imagePath, picture.Image.ImageBytes);
                        //将picture存到文件
                        /*
                        using (var fileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
                        {
                            fileStream.Write(picture.Image.ImageBytes, 0, picture.Image.ImageBytes.Length);
                        }
                        */
                    }
                }
            }
        }
    }
}
