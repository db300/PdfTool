﻿using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

namespace PdfHelperLibraryX
{
    /// <summary>
    /// 图片提取帮助类
    /// </summary>
    public static class ImageExtractHelper
    {
        public static string ExportImage(string inputPdfFileName)
        {
            try
            {
                var document = PdfReader.Open(inputPdfFileName);
                var imageCount = 0;
                // Iterate pages
                foreach (var page in document.Pages)
                {
                    // Get resources dictionary
                    var resources = page.Elements.GetDictionary("/Resources");
                    // Get external objects dictionary
                    var xObjects = resources?.Elements.GetDictionary("/XObject");
                    if (xObjects == null) continue;
                    var items = xObjects.Elements.Values;
                    // Iterate references to external objects
                    foreach (var item in items)
                    {
                        if (!(item is PdfReference reference)) continue;
                        // Is external object an image?
                        if (reference.Value is PdfDictionary xObject && xObject.Elements.GetString("/Subtype") == "/Image") ExportImage(xObject, inputPdfFileName, ref imageCount);
                    }
                }
                return $"提取 {imageCount} 张图片.";
            }
            catch (Exception ex)
            {
                return $"提取失败，原因：{ex.Message}";
            }
        }

        public static List<string> ExportImages(List<string> fileNames)
        {
            var outFileList = new List<string>();
            foreach (var fileName in fileNames)
            {
                try
                {
                    var document = PdfReader.Open(fileName);
                    var imageCount = 0;
                    // Iterate pages
                    foreach (var page in document.Pages)
                    {
                        // Get resources dictionary
                        var resources = page.Elements.GetDictionary("/Resources");
                        // Get external objects dictionary
                        var xObjects = resources?.Elements.GetDictionary("/XObject");
                        if (xObjects == null) continue;
                        var items = xObjects.Elements.Values;
                        // Iterate references to external objects
                        foreach (var item in items)
                        {
                            if (!(item is PdfReference reference)) continue;
                            // Is external object an image?
                            if (reference.Value is PdfDictionary xObject && xObject.Elements.GetString("/Subtype") == "/Image") outFileList.Add(ExportImage(xObject, fileName, ref imageCount));
                        }
                    }
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($@"{fileName} export {imageCount} images.");
#endif
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
#endif
                    File.Move(fileName, Path.GetFileName(fileName));
                }
            }
            return outFileList.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        }

        private static string ExportImage(PdfDictionary image, string fileNameWithoutExt, ref int count)
        {
            var filter = image.Elements["/Filter"].ToString().Replace("[", "").Replace("]", "").Trim();
            switch (filter)
            {
                case "/DCTDecode":
                    return ExportJpegImage(image, fileNameWithoutExt, ref count);
                case "/FlateDecode":
                    ExportAsPngImage(image, ref count);
                    break;
            }
            return "";
        }

        private static string ExportJpegImage(PdfDictionary image, string fileNameWithoutExt, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            var stream = image.Stream.Value;
            var outFileName = $"{fileNameWithoutExt}_{count++}.jpeg";
            var fs = new FileStream(outFileName, FileMode.Create, FileAccess.Write);
            var bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();
            return outFileName;
        }

        private static void ExportAsPngImage(PdfDictionary image, ref int count)
        {
            var width = image.Elements.GetInteger(PdfImage.Keys.Width);
            var height = image.Elements.GetInteger(PdfImage.Keys.Height);
            var bitsPerComponent = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent);

            // TODO: You can put the code here that converts vom PDF internal image format to a Windows bitmap
            // and use GDI+ to save it in PNG format.
            // It is the work of a day or two for the most important formats. Take a look at the file
            // PdfSharp.Pdf.Advanced/PdfImage.cs to see how we create the PDF image formats.
            // We don't need that feature at the moment and therefore will not implement it.
            // If you write the code for exporting images I would be pleased to publish it in a future release
            // of PDFsharp.
        }
    }
}