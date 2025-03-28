using Aspose.Pdf.Devices;
using Aspose.Pdf;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfImageExtractConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string filename = "test.pdf";
            const string folder = "images";

            ExtractImages1(filename, folder);
        }

        public static void ExtractImages1(string pdfPath, string outputFolder)
        {
            // 打开PDF文档
            Document pdfDocument = new Document(pdfPath);

            // 遍历每一页
            for (int pageCount = 1; pageCount <= pdfDocument.Pages.Count; pageCount++)
            {
                Page page = pdfDocument.Pages[pageCount];

                // 遍历页面中的所有图像
                for (int imageCount = 1; imageCount <= page.Resources.Images.Count; imageCount++)
                {
                    // 提取图像
                    XImage xImage = page.Resources.Images[imageCount];

                    // 创建输出文件路径
                    string outputFilePath = Path.Combine(outputFolder, $"Image_{pageCount}_{imageCount}.png");

                    // 创建图像设备
                    using (FileStream imageStream = new FileStream(outputFilePath, FileMode.Create))
                    {
                        // 设置图像设备的属性
                        PngDevice pngDevice = new PngDevice();
                        pngDevice.Process(page, imageStream);
                        imageStream.Close();
                    }
                }
            }
        }

        /*
        public static void ExtractImages(string pdfPath, string outputFolder)
        {
            using (PdfReader reader = new PdfReader(pdfPath))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfDictionary pageDict = reader.GetPageN(i);
                    PdfDictionary resources = (PdfDictionary)PdfReader.GetPdfObject(pageDict.Get(PdfName.RESOURCES));
                    PdfDictionary xObject = (PdfDictionary)PdfReader.GetPdfObject(resources.Get(PdfName.XOBJECT));

                    if (xObject != null)
                    {
                        foreach (PdfName name in xObject.Keys)
                        {
                            PdfObject obj = xObject.Get(name);
                            if (obj.IsIndirect())
                            {
                                PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                                PdfName subtype = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));

                                if (PdfName.IMAGE.Equals(subtype))
                                {
                                    int xrefIndex = ((PRIndirectReference)obj).Number;
                                    PdfObject pdfObj = reader.GetPdfObject(xrefIndex);
                                    PdfStream pdfStream = (PdfStream)pdfObj;
                                    byte[] bytes = PdfReader.GetStreamBytesRaw((PRStream)pdfStream);

                                    if (bytes != null)
                                    {
                                        using (MemoryStream memStream = new MemoryStream(bytes))
                                        {
                                            memStream.Position = 0;
                                            try
                                            {
                                                Image img = Image.FromStream(memStream);
                                                img.Save(Path.Combine(outputFolder, $"Image_{i}_{name}.png"), ImageFormat.Png);
                                            }
                                            catch (ArgumentException ex)
                                            {
                                                Console.WriteLine($"Failed to create image from stream: {ex.Message}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        */
    }
}
