using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using System.Drawing;
using System.Drawing.Imaging;
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
                using (var sealImg = Image.FromFile(sealImgFileName))
                using (var scaledSealImg = ScaleImage(sealImg, scale))
                {
                    int pageCount = pdf.PageCount;
                    int partWidth = scaledSealImg.Width / pageCount;
                    for (int i = 0; i < pageCount; i++)
                    {
                        using (var partImg = CropImage(scaledSealImg, i * partWidth, 0, partWidth, scaledSealImg.Height))
                        using (var ms = new MemoryStream())
                        {
                            partImg.Save(ms, ImageFormat.Png);
                            ms.Position = 0;
                            var xImg = XImage.FromStream(ms);

                            DrawSealOnPage(pdf.Pages[i], xImg);
                        }
                    }
                }

                string outputFile = Path.Combine(
                    Path.GetDirectoryName(intputPdfFileName),
                    $"{Path.GetFileNameWithoutExtension(intputPdfFileName)}_sealed.pdf");
                pdf.Save(outputFile);
            }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        private static Bitmap ScaleImage(Image img, double scale)
        {
            int width = (int)(img.Width * scale);
            int height = (int)(img.Height * scale);
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(img, 0, 0, width, height);
            }
            return bmp;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        private static Bitmap CropImage(Bitmap src, int x, int y, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(src, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }
            return bmp;
        }

        /// <summary>
        /// 绘制印章到页面
        /// </summary>
        private static void DrawSealOnPage(PdfSharp.Pdf.PdfPage page, XImage xImg)
        {
            double x = page.Width - xImg.PointWidth;
            double y = (page.Height - xImg.PointHeight) / 2 - 100;
            using (var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append))
            {
                gfx.SmoothingMode = XSmoothingMode.HighQuality;
                gfx.DrawImage(xImg, x, y, xImg.PointWidth, xImg.PointHeight);
            }
        }
    }
}
