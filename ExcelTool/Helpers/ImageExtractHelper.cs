using OfficeOpenXml.Drawing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExcelTool.Helpers
{
    /// <summary>
    /// 图片提取帮助类
    /// </summary>
    internal static class ImageExtractHelper
    {
        internal static void ExtractImages(string excelFilePath, CommonHelper.InfoHandler infoHandler)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(excelFilePath)))
            {
                var workbook = package.Workbook;
                foreach (var worksheet in workbook.Worksheets)
                {
                    infoHandler($"正在提取 {excelFilePath} 的工作表 {worksheet.Name}");

                    var images = new List<(ExcelPicture picture, int row, int column)>();

                    var drawings = worksheet.Drawings;
                    foreach (var drawing in drawings)
                    {
                        if (drawing is ExcelPicture picture)
                        {
                            var row = picture.From.Row + 1; // EPPlus uses 0-based index
                            var column = picture.From.Column + 1; // EPPlus uses 0-based index
                            images.Add((picture, row, column));
                        }
                    }

                    infoHandler($"提取到 {images.Count} 张图片");

                    if (images.Count <= 0) continue;

                    var outputPath = Path.Combine(Path.GetDirectoryName(excelFilePath), Path.GetFileNameWithoutExtension(excelFilePath).Trim());
                    if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);

                    infoHandler($"正在保存至 {outputPath}");

                    foreach (var (picture, row, column) in images)
                    {
                        Console.WriteLine($"Image at Row: {row}, Column: {column}");
                        // Save the image to a file or process it as needed
                        var imagePath = Path.Combine(outputPath, $"{worksheet.Name}_Image_{row}_{column}.png");
                        File.WriteAllBytes(imagePath, picture.Image.ImageBytes);
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
