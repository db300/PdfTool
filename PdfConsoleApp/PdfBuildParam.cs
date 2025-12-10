namespace PdfConsoleApp
{
    /// <summary>
    /// PDF 生成参数
    /// </summary>
    internal class PdfBuildParam
    {
        /// <summary>
        /// 输出的 PDF 文件名
        /// </summary>
        public string OutputFileName { get; set; }

        /// <summary>
        /// PDF 的文本内容
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// 字体名称（可选）
        /// </summary>
        public string FontName { get; set; } = "宋体";

        /// <summary>
        /// 字体大小（可选）
        /// </summary>
        public double FontSize { get; set; } = 12;

        /// <summary>
        /// 页面宽度（可选，默认 A4 宽度）
        /// </summary>
        public double PageWidth { get; set; } = 595;

        /// <summary>
        /// 页面高度（可选，默认 A4 高度）
        /// </summary>
        public double PageHeight { get; set; } = 842;

        /// <summary>
        /// 左边距（可选）
        /// </summary>
        public double MarginLeft { get; set; } = 40;

        /// <summary>
        /// 右边距（可选）
        /// </summary>
        public double MarginRight { get; set; } = 40;

        /// <summary>
        /// 上边距（可选）
        /// </summary>
        public double MarginTop { get; set; } = 40;

        /// <summary>
        /// 下边距（可选）
        /// </summary>
        public double MarginBottom { get; set; } = 40;
    }
}
