﻿using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PdfHelperLibrary
{
    /// <summary>
    /// 图像化帮助类
    /// </summary>
    public static class ImagerHelper
    {
        /// <summary>
        /// PDF转图片(全页)
        /// </summary>
        /// <param name="inputPdfFileName"></param>
        /// <param name="dpi">转换图片dpi</param>
        public static string ConvertPdfToImage(string inputPdfFileName, int dpi, InfoHandler handler)
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
                    var outputFileName = $"{outputFileNamePrefix}-page{(i + 1).ToString().PadLeft(pageNumLength, '0')}.png";
                    image.Save(outputFileName, ImageFormat.Png);
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
            _dict = new Dictionary<int, Bitmap>();
        }

        private readonly PDFFile _file;
        private readonly Dictionary<int, Bitmap> _dict;

        public Bitmap GetPageImage(int pageNum, int dpi)
        {
            if (_dict.ContainsKey(pageNum)) return _dict[pageNum];
            var img = _file.GetPageImage(pageNum, dpi);
            _dict.Add(pageNum, img);
            return img;
        }
    }
}
