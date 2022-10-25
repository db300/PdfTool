using PdfSharp.Charting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 水印帮助类
    /// </summary>
    public static class WatermarkHelper
    {
        private const int _emSize = 30;
        private static readonly XFont _font = new XFont("黑体", _emSize, XFontStyle.BoldItalic);

        public static string WatermarkPdf(string inputPdfFileName, string watermark)
        {
            try
            {
                // Get a fresh copy of the input PDF file.
                var path = Path.GetDirectoryName(inputPdfFileName);
                var fileName = Path.GetFileNameWithoutExtension(inputPdfFileName);
                var prefixFileName = Path.Combine(path, fileName);
                var outputPdfFileName = $"{prefixFileName}-WatermarkFile - {DateTime.Now:yyyyMMddHHmmssfff}.pdf";
                File.Copy(inputPdfFileName, outputPdfFileName, true);
                // Remove ReadOnly attribute from the copy.
                File.SetAttributes(outputPdfFileName, File.GetAttributes(outputPdfFileName) & ~FileAttributes.ReadOnly);

                var inputDocument = PdfReader.Open(outputPdfFileName);
                foreach (var page in inputDocument.Pages)
                {
                    Watermark1(page, watermark);
                }
                inputDocument.Save(outputPdfFileName);
                Process.Start(outputPdfFileName);
                return "";
            }
            catch (Exception ex)
            {
                return $"水印添加失败，原因：{ex.Message}";
            }
        }

        private static void Watermark1(PdfPage page, string watermark)
        {
            // Variation 1: Draw a watermark as a text string.

            // Get an XGraphics object for drawing beneath the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

            // Get the size (in points) of the text.
            var size = gfx.MeasureString(watermark, _font);

            // Define a rotation transformation at the center of the page.
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

            // Create a string format.
            var format = new XStringFormat
            {
                Alignment = XStringAlignment.Near,
                LineAlignment = XLineAlignment.Near
            };

            // Create a dimmed red brush.
            XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));

            // Draw the string.
            gfx.DrawString(watermark, _font, brush, new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);

            gfx.Save();
        }

        private static void Watermark2(PdfPage page, string watermark)
        {
            // Variation 2: Draw a watermark as an outlined graphical path.
            // NYI: Does not work in Core build.

            // Get an XGraphics object for drawing beneath the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

            // Get the size (in points) of the text.
            var size = gfx.MeasureString(watermark, _font);

            // Define a rotation transformation at the center of the page.
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

            // Create a graphical path.
            var path = new XGraphicsPath();

            // Create a string format.
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Add the text to the path.
            // AddString is not implemented in PDFsharp Core.
            path.AddString(watermark, _font.FontFamily, XFontStyle.BoldItalic, 150, new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);

            // Create a dimmed red pen.
            var pen = new XPen(XColor.FromArgb(128, 255, 0, 0), 2);

            // Stroke the outline of the path.
            gfx.DrawPath(pen, path);

            gfx.Save();
        }

        private static void Watermark3(PdfPage page, string watermark)
        {
            // Variation 3: Draw a watermark as a transparent graphical path above text.
            // NYI: Does not work in Core build.

            // Get an XGraphics object for drawing above the existing content.
            var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);

            // Get the size (in points) of the text.
            var size = gfx.MeasureString(watermark, _font);

            // Define a rotation transformation at the center of the page.
            gfx.TranslateTransform(page.Width / 2, page.Height / 2);
            gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
            gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

            // Create a graphical path.
            var path = new XGraphicsPath();

            // Create a string format.
            var format = new XStringFormat();
            format.Alignment = XStringAlignment.Near;
            format.LineAlignment = XLineAlignment.Near;

            // Add the text to the path.
            // AddString is not implemented in PDFsharp Core.
            path.AddString(watermark, _font.FontFamily, XFontStyle.BoldItalic, 150, new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);

            // Create a dimmed red pen and brush.
            var pen = new XPen(XColor.FromArgb(50, 75, 0, 130), 3);
            XBrush brush = new XSolidBrush(XColor.FromArgb(50, 106, 90, 205));

            // Stroke the outline of the path.
            gfx.DrawPath(pen, brush, path);

            gfx.Save();
        }
    }
}
