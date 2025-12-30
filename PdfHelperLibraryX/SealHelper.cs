using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 印章帮助类
    /// </summary>
    public static class SealHelper
    {
        /// <summary>
        /// 骑缝章
        /// </summary>
        /// <param name="sealImgFileName">印章图片文件名</param>
        /// <param name="intputPdfFileName">输入PDF文件名</param>
        /// <param name="scale">印章图片缩放比例</param>
        public static void CrossPageSeal(string sealImgFileName, string intputPdfFileName, double scale = 1.0)
        {
            using (var pdf = PdfReader.Open(intputPdfFileName, PdfDocumentOpenMode.Modify))
            {
                using (var sealImg = Image.Load<Rgba32>(sealImgFileName))
                using (var scaledSealImg = ScaleImage(sealImg, scale))
                {
                    int pageCount = pdf.PageCount;
                    int partWidth = scaledSealImg.Width / pageCount;
                    for (int i = 0; i < pageCount; i++)
                    {
                        using (var partImg = CropImage(scaledSealImg, i * partWidth, 0, partWidth, scaledSealImg.Height))
                        using (var ms = new MemoryStream())
                        {
                            partImg.SaveAsPng(ms);
                            ms.Position = 0;
                            var xImg = XImage.FromStream(ms);

                            DrawSealOnPage(pdf.Pages[i], xImg);
                        }
                    }
                }

                string outputFile = Path.Combine(
                    Path.GetDirectoryName(intputPdfFileName),
                    $"{Path.GetFileNameWithoutExtension(intputPdfFileName)}_sealed_{DateTime.Now.Ticks}.pdf");
                pdf.Save(outputFile);
            }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        private static Image<Rgba32> ScaleImage(Image<Rgba32> img, double scale)
        {
            int width = (int)(img.Width * scale);
            int height = (int)(img.Height * scale);
            var clone = img.Clone(ctx => ctx.Resize(width, height));
            return clone;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        private static Image<Rgba32> CropImage(Image<Rgba32> src, int x, int y, int width, int height)
        {
            var clone = src.Clone(ctx => ctx.Crop(new SixLabors.ImageSharp.Rectangle(x, y, width, height)));
            return clone;
        }

        /// <summary>
        /// 绘制印章到页面
        /// </summary>
        private static void DrawSealOnPage(PdfSharp.Pdf.PdfPage page, XImage xImg)
        {
            double x = page.MediaBox.Width - xImg.PointWidth;
            double y = (page.MediaBox.Height - xImg.PointHeight) / 2 - 100;
            using (var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append))
            {
                gfx.SmoothingMode = XSmoothingMode.HighQuality;
                gfx.DrawImage(xImg, x, y, xImg.PointWidth, xImg.PointHeight);
            }
        }
    }
}
