using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;

namespace WordHelperLibrary
{
    /// <summary>
    /// 定制帮助类
    /// </summary>
    public static class CustomHelper
    {
        public static void Build1(string inputFileName, Dictionary<string, string> bodyReplaceDict, Dictionary<string, string> headerReplaceDict, string qrCodeFileName)
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
                if (!string.IsNullOrWhiteSpace(qrCodeFileName))
                {
                    ReplaceHelper.ReplaceImageInBody(tempDoc, qrCodeFileName);
                }
            }
        }
    }
}
