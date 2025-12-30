using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 生成器
    /// </summary>
    public class Builder
    {
        public static void Image2Pdf(List<string> imgFileList, string outFileName)
        {
            using (var pdf = new PdfDocument())
            {
                for (var i = 0; i < imgFileList.Count; i++)
                {
                    var page = pdf.AddPage();
                    var graphics = XGraphics.FromPdfPage(page);

                    var image = XImage.FromFile(imgFileList[i]);
                    if (image.PixelWidth > image.PixelHeight) page.Orientation = PdfSharp.PageOrientation.Landscape;

                    graphics.DrawImage(image, 0, 0, page.Width.Point, page.Height.Point);
                    //graphics.DrawImage(image, 0, 0);
                }
                pdf.Save(outFileName);
            }
        }

        public static void InsertImage2Pdf(string inFileName, List<string> imgFileList, string outFileName)
        {
            using (var pdf = PdfReader.Open(inFileName))
            {
                for (var i = 0; i < imgFileList.Count; i++)
                {
                    var page = pdf.AddPage();
                    var graphics = XGraphics.FromPdfPage(page);

                    var image = XImage.FromFile(imgFileList[i]);
                    if (image.PixelWidth > image.PixelHeight) page.Orientation = PdfSharp.PageOrientation.Landscape;

                    graphics.DrawImage(image, 0, 0, page.Width.Point, page.Height.Point);
                    //graphics.DrawImage(image, 0, 0);
                }
                pdf.Save(outFileName);
            }
        }

        /// <summary>
        /// 根据文本内容生成 PDF 文件
        /// </summary>
        public static void Text2Pdf(string text, string outputFileName, string fontName = "宋体", double fontSize = 12,
            double pageWidth = 595, double pageHeight = 842,
            double marginLeft = 40, double marginRight = 40, double marginTop = 40, double marginBottom = 40)
        {
            using (var document = new PdfDocument())
            {
                // 创建页面
                var page = document.AddPage();
                page.Width = XUnit.FromPoint(pageWidth);
                page.Height = XUnit.FromPoint(pageHeight);

                // 创建图形对象
                var gfx = XGraphics.FromPdfPage(page);

                // 创建字体 - 使用 XPdfFontOptions.UnicodeDefault 支持中文
                var options = new XPdfFontOptions(PdfFontEncoding.Unicode);
                var font = new XFont(fontName, fontSize, XFontStyleEx.Regular, options);

                // 计算可用区域
                var maxWidth = pageWidth - marginLeft - marginRight;
                var maxHeight = pageHeight - marginTop - marginBottom;

                // 绘制文本
                DrawText(gfx, text, font, marginLeft, marginTop, maxWidth, maxHeight);

                // 保存 PDF
                document.Save(outputFileName);
            }
        }

        /// <summary>
        /// 绘制文本，支持自动换行
        /// </summary>
        private static void DrawText(XGraphics gfx, string text, XFont font, double x, double y, double maxWidth, double maxHeight)
        {
            // 首先按明确的换行符分割文本
            var paragraphs = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var currentY = y;
            var lineHeight = font.GetHeight();

            foreach (var paragraph in paragraphs)
            {
                // 对每个段落进行自动换行处理
                var wrappedLines = WrapText(gfx, paragraph, font, maxWidth);

                foreach (var line in wrappedLines)
                {
                    // 检查是否超出页面高度
                    if (currentY + lineHeight > y + maxHeight)
                    {
                        break;
                    }

                    gfx.DrawString(line, font, XBrushes.Black, new XRect(x, currentY, maxWidth, lineHeight), XStringFormats.TopLeft);
                    currentY += lineHeight;
                }

                // 如果已超出页面高度，停止绘制
                if (currentY + lineHeight > y + maxHeight)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 对文本进行自动换行处理
        /// </summary>
        /// <param name="gfx">图形对象</param>
        /// <param name="text">要换行的文本</param>
        /// <param name="font">字体</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <returns>换行后的文本行列表</returns>
        private static List<string> WrapText(XGraphics gfx, string text, XFont font, double maxWidth)
        {
            var wrappedLines = new List<string>();

            // 如果文本为空，返回空列表
            if (string.IsNullOrEmpty(text))
            {
                wrappedLines.Add(string.Empty);
                return wrappedLines;
            }

            var currentLine = string.Empty;
            var charIndex = 0;

            while (charIndex < text.Length)
            {
                var currentChar = text[charIndex].ToString();
                var testLine = currentLine + currentChar;

                // 测量当前文本的宽度
                var textSize = gfx.MeasureString(testLine, font);

                // 如果当前行加上新字符仍在宽度范围内，继续添加
                if (textSize.Width <= maxWidth)
                {
                    currentLine = testLine;
                    charIndex++;
                }
                else
                {
                    // 如果当前行不为空，将其添加到结果列表中
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        wrappedLines.Add(currentLine);
                        currentLine = string.Empty;
                    }
                    else
                    {
                        // 如果当前行为空但单个字符就超过宽度，强制添加该字符
                        wrappedLines.Add(currentChar);
                        charIndex++;
                    }
                }
            }

            // 添加最后一行
            if (!string.IsNullOrEmpty(currentLine))
            {
                wrappedLines.Add(currentLine);
            }

            return wrappedLines;
        }
    }
}
