using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                    graphics.DrawImage(image, 0, 0, page.Width, page.Height);
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

                    graphics.DrawImage(image, 0, 0, page.Width, page.Height);
                    //graphics.DrawImage(image, 0, 0);
                }
                pdf.Save(outFileName);
            }
        }
    }
}
