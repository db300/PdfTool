using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;

namespace WordHelperLibrary
{
    /// <summary>
    /// 定制帮助类
    /// </summary>
    public static class CustomHelper
    {
        public static void Build1(string inputFileName,
            Dictionary<string, string> bodyReplaceDict,
            Dictionary<string, string> headerReplaceDict,
            string qrCodeImgEmbedId, string qrCodeFileName,
            string fontUseImgEmbedId, string fontUseQRCodeFileName)
        {
            using (var tempDoc = WordprocessingDocument.Open(inputFileName, true))
            {
                //替换正文内容
                ReplaceHelper.ReplaceTextInBody(tempDoc, bodyReplaceDict);
                //替换页眉内容
                foreach (var headerPart in tempDoc.MainDocumentPart.HeaderParts)
                {
                    ReplaceHelper.ReplaceTextInHeader(headerPart, headerReplaceDict);
                }
                //替换图片
                if (!string.IsNullOrWhiteSpace(qrCodeFileName) && !string.IsNullOrWhiteSpace(qrCodeImgEmbedId))
                {
                    ReplaceHelper.ReplaceImageInBody(tempDoc, qrCodeImgEmbedId, qrCodeFileName);
                }
                if (!string.IsNullOrWhiteSpace(fontUseQRCodeFileName) && !string.IsNullOrWhiteSpace(fontUseImgEmbedId))
                {
                    ReplaceHelper.ReplaceImageInBody(tempDoc, fontUseImgEmbedId, fontUseQRCodeFileName);
                }
            }
        }
    }
}
