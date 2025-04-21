using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 图像化帮助类
    /// </summary>
    public static class ImagerHelper
    {
        private static readonly Dictionary<string, ImageFormat> ImgFormatDict = new Dictionary<string, ImageFormat>
        {
            {"png", ImageFormat.Png},
            {"jpg", ImageFormat.Jpeg},
            {"jpeg", ImageFormat.Jpeg},
            {"bmp", ImageFormat.Bmp},
            {"gif", ImageFormat.Gif},
            {"tiff", ImageFormat.Tiff},
            {"tif", ImageFormat.Tiff},
            {"emf", ImageFormat.Emf},
            {"wmf", ImageFormat.Wmf}
        };

        /// <summary>
        /// PDF转图片(全页)
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="dpi">转换图片dpi</param>
        /// <param name="ext">生成图片扩展名</param>
        /// <param name="handler">消息事件句柄</param>
        public static string ConvertPdfToImage(string inputPdfFileName, int dpi, string ext, InfoHandler handler)
        {
            try
            {
                //const int dpi = 600;//set image dpi
                var file = PDFFile.Open(inputPdfFileName);//open pdf file

                var pageCount = file.PageCount;
                var pageNumLength = pageCount.ToString().Length;
                var outputFileNamePrefix = Path.Combine(Path.GetDirectoryName(inputPdfFileName), Path.GetFileNameWithoutExtension(inputPdfFileName));

                for (var i = 0; i < pageCount; i++)
                {
                    var image = file.GetPageImage(i, dpi);//get pdf image
                    var outputFileName = $"{outputFileNamePrefix}-page{(i + 1).ToString().PadLeft(pageNumLength, '0')}.{ext}";
                    image.Save(outputFileName, ImgFormatDict[ext]);
                    handler?.Invoke($"已转换第{i + 1}页");
                }
                return "";
            }
            catch (Exception ex)
            {
                return $"转换失败，原因：{ex.Message}";
            }
        }

        /// <summary>
        /// PDF转图片(指定页)
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="dpi">转换图片dpi</param>
        /// <param name="ext">生成图片扩展名</param>
        /// <param name="pageNums">页码列表，从1开始</param>
        /// <param name="handler">消息事件句柄</param>
        public static string ConvertPdfToImage(string inputPdfFileName, int dpi, string ext, List<int> pageNums, InfoHandler handler)
        {
            try
            {
                //const int dpi = 600;//set image dpi
                var file = PDFFile.Open(inputPdfFileName);//open pdf file

                var pageCount = file.PageCount;
                var pageNumLength = pageCount.ToString().Length;
                var outputFileNamePrefix = Path.Combine(Path.GetDirectoryName(inputPdfFileName), Path.GetFileNameWithoutExtension(inputPdfFileName));

                var validPageNums = pageNums.Where(a => a > 0 && a <= pageCount).ToList();
                validPageNums.Sort();

                foreach (var pageNum in validPageNums)
                {
                    var image = file.GetPageImage(pageNum - 1, dpi);//get pdf image
                    var outputFileName = $"{outputFileNamePrefix}-page{pageNum.ToString().PadLeft(pageNumLength, '0')}.{ext}";
                    image.Save(outputFileName, ImgFormatDict[ext]);
                    handler?.Invoke($"已转换第{pageNum}页");
                }
                return "";
            }
            catch (Exception ex)
            {
                return $"转换失败，原因：{ex.Message}";
            }
        }

        /// <summary>
        /// PDF转图片(单页)
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="pageNum">指定页(起始0)</param>
        /// <param name="outputFolder"></param>
        public static string ConvertPdfToImage(string inputPdfFileName, int pageNum, string outputFolder)
        {
            try
            {
                const int dpi = 300;//set image dpi
                using (var file = PDFFile.Open(inputPdfFileName))//open pdf file
                {
                    using (var image = file.GetPageImage(pageNum, dpi))//get pdf image
                    {
                        var outputFileName = Path.Combine(outputFolder, $"{Path.GetFileNameWithoutExtension(inputPdfFileName)}-page{pageNum + 1}.png");
                        image.Save(outputFileName, ImageFormat.Png);
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return $"转换失败，原因：{ex.Message}";
            }
        }

        public delegate void InfoHandler(string info);
    }

    public class ImagerHelper2
    {
        public ImagerHelper2(string inputPdfFileName)
        {
            _file = PDFFile.Open(inputPdfFileName);
            _dict = new Dictionary<string, Bitmap>();
            PageCount = _file.PageCount;
        }

        private readonly PDFFile _file;
        private readonly Dictionary<string, Bitmap> _dict;
        public readonly int PageCount;

        public Bitmap GetPageImage(int pageNum, int dpi)
        {
            var key = $"{pageNum}_{dpi}";
            if (_dict.ContainsKey(key)) return _dict[key];
            var img = _file.GetPageImage(pageNum, dpi);
            _dict.Add(key, img);
            return img;
        }

        public Bitmap GetPageImage(int pageNum, int dpi, int rotationAngle)
        {
            var key = $"{pageNum}_{dpi}_{rotationAngle}";
            if (_dict.ContainsKey(key)) return _dict[key];
            var img = _file.GetPageImage(pageNum, dpi);
            var rotatedImage = (Bitmap)img.Clone();

            RotateFlipType rotateFlipType;
            switch (rotationAngle)
            {
                case 0:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 90:
                case -270:// 逆时针 -270 等同于顺时针 90
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 180:
                case -180:// 逆时针 -180 等同于顺时针 180
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 270:
                case -90:// 逆时针 -90 等同于顺时针 270
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    throw new ArgumentException($"Invalid rotation angle: {rotationAngle}");
            }
            rotatedImage.RotateFlip(rotateFlipType);
            _dict.Add(key, rotatedImage);
            return rotatedImage;
        }
    }
}
