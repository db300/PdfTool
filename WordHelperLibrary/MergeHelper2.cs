using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;

namespace WordHelperLibrary
{
    public static class MergeHelper2
    {
        [Obsolete("临时测试方法，只能合并正文内容，不能合并页眉页脚等内容", false)]
        public static string Merge(List<string> inputFilenameList, string outputFileName)
        {
            try
            {
                using (var outputDoc = WordprocessingDocument.Create(outputFileName, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
                {
                    var mainPart = outputDoc.AddMainDocumentPart();
                    mainPart.Document = new Document(new Body());

                    foreach (var inputFile in inputFilenameList)
                    {
                        using (var tempDoc = WordprocessingDocument.Open(inputFile, true))
                        {
                            var body = tempDoc.MainDocumentPart.Document.Body;
                            foreach (var element in body.Elements())
                            {
                                mainPart.Document.Body.Append(element.CloneNode(true));
                            }
                        }
                    }

                    mainPart.Document.Save();
                }
                return "";
            }
            catch (Exception ex)
            {
                return $"合并失败，原因：{ex.Message}";
            }
        }
    }
}
