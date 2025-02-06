using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImageHelper
    {
        public static string ConvertImageToPdf(List<string> inputImgFileNames, out string outputPdfFileName)
        {
            try
            {
                var outputDocument = new PdfDocument();
                foreach (var file in inputImgFileNames)
                {
                    // Create an empty page or load existing
                    var page = outputDocument.AddPage();

                    // Get an XGraphics object for drawing
                    var gfx = XGraphics.FromPdfPage(page);

                    var image = XImage.FromFile(file);
                    page.Width = image.PointWidth;
                    page.Height = image.PointHeight;
                    gfx.DrawImage(image, 0, 0);
                }
                var path = Path.GetDirectoryName(inputImgFileNames.First());
                outputPdfFileName = Path.Combine(path, $"MergedFile - {DateTime.Now:yyyyMMddHHmmssfff}.pdf");
                outputDocument.Save(outputPdfFileName);
                return "";
            }
            catch (Exception ex)
            {
                outputPdfFileName = "";
                return $"生成失败，原因：{ex.Message}";
            }
        }
    }
}
