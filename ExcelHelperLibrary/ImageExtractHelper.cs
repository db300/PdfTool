using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHelperLibrary
{
    /// <summary>
    /// 图片提取帮助类
    /// </summary>
    public static class ImageExtractHelper
    {
        public static void ExtractImages(string excelFilePath)
        {
            try
            {
                using (var fs = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);
                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);
                        var images = new List<(byte[] imageData, int row, int column)>();

                        var drawingPatriarch = sheet.DrawingPatriarch as XSSFDrawing;
                        if (drawingPatriarch != null)
                        {
                            foreach (var shape in drawingPatriarch.GetShapes())
                            {
                                if (shape is XSSFPicture xssfPicture)
                                {
                                    var anchor = xssfPicture.GetPreferredSize();
                                    var row = anchor.Row1 + 1; // NPOI uses 0-based index
                                    var column = anchor.Col1 + 1; // NPOI uses 0-based index
                                    images.Add((xssfPicture.PictureData.Data, row, column));
                                }
                            }
                        }

                        foreach (var (imageData, row, column) in images)
                        {
                            Console.WriteLine($"Image at Row: {row}, Column: {column}");
                            // Save the image to a file or process it as needed
                            var imagePath = Path.Combine("output", $"{sheet.SheetName}_Image_{row}_{column}.png");
                            Directory.CreateDirectory("output");
                            File.WriteAllBytes(imagePath, imageData);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
