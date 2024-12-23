using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordHelperLibrary
{
    /// <summary>
    /// 替换帮助类
    /// </summary>
    public static class ReplaceHelper
    {
        /// <summary>
        /// 替换页眉的文本
        /// </summary>
        /// <param name="headerPart"></param>
        /// <param name="oldText"></param>
        /// <param name="newText"></param>
        public static void ReplaceTextInHeader(HeaderPart headerPart, string oldText, string newText)
        {
            string content;
            using (var sr = new StreamReader(headerPart.GetStream()))
            {
                content = sr.ReadToEnd();
            }
            content = content.Replace(oldText, newText);
            using (var sw = new StreamWriter(headerPart.GetStream(FileMode.Create)))
            {
                sw.Write(content);
            }
        }

        /// <summary>
        /// 替换正文的文本
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="replacements">{oldText, newText}</param>
        public static void ReplaceTextInBody(WordprocessingDocument wordDoc, Dictionary<string, string> replacements)
        {
            var body = wordDoc.MainDocumentPart.Document.Body;
            var texts = body.Descendants<Text>();
            foreach (var text in texts)
            {
                foreach (var item in replacements)
                {
                    if (text.Text.Contains(item.Key))
                    {
                        text.Text = text.Text.Replace(item.Key, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// 替换正文中的图片
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="newImagePath"></param>
        public static void ReplaceImageInBody(WordprocessingDocument wordDoc, string newImagePath)
        {
            var mainPart = wordDoc.MainDocumentPart;
            var oldImagePart = mainPart.ImageParts.FirstOrDefault();
            if (oldImagePart != null)
            {
                var contentType = oldImagePart.ContentType;
                mainPart.DeletePart(oldImagePart);

                var newImagePart = mainPart.AddImagePart(contentType);
                using (var stream = new FileStream(newImagePath, FileMode.Open, FileAccess.Read))
                {
                    newImagePart.FeedData(stream);
                }

                var blipElements = mainPart.Document.Descendants<DocumentFormat.OpenXml.Drawing.Blip>();
                foreach (var blip in blipElements)
                {
                    blip.Embed.Value = mainPart.GetIdOfPart(newImagePart);
                }
            }
        }
    }
}
