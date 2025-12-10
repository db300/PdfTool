namespace PdfConsoleApp
{
    /// <summary>
    /// PDF 生成器
    /// </summary>
    internal static class PdfBuilder
    {
        /// <summary>
        /// 根据参数生成 PDF 文件
        /// </summary>
        public static void GeneratePdf(PdfBuildParam param)
        {
            PdfHelperLibrary.Builder.Text2Pdf(
                param.TextContent,
                param.OutputFileName,
                param.FontName,
                param.FontSize,
                param.PageWidth,
                param.PageHeight,
                param.MarginLeft,
                param.MarginRight,
                param.MarginTop,
                param.MarginBottom
            );
        }
    }
}
