using UglyToad.PdfPig;

namespace PdfHelperLibraryX
{
    /// <summary>
    /// 文本提取帮助类
    /// </summary>
    public static class TextExtractHelper
    {
        public static List<string> ExtractText(string inputPdfFileName)
        {
            var list = new List<string>();
            using (var document = PdfDocument.Open(inputPdfFileName))
            {
                var pages = document.GetPages();
                foreach (var page in pages)
                {
                    var pageText = page.Text;
                    list.Add(pageText);

                    var words = page.GetWords();
                    foreach (var word in words)
                    {
#if DEBUG
                        System.Diagnostics.Debug.WriteLine(word.Text);
#endif
                    }
                }
            }
            return list;
        }
    }
}
