namespace UkuleleEditor
{
    /// <summary>
    /// 曲谱实体
    /// </summary>
    internal class SheetMusicItem
    {
        /// <summary>
        /// 简谱（1-7，0表示休止符）
        /// </summary>
        public int Notation { get; set; }

        /// <summary>
        /// 弦
        /// </summary>
        public int String { get; set; }
        /// <summary>
        /// 品
        /// </summary>
        public int Fret { get; set; }
        /// <summary>
        /// 词
        /// </summary>
        public string? Lyric { get; set; }
    }

    /// <summary>
    /// 曲谱行
    /// </summary>
    internal class SheetMusicLine
    {
        /// <summary>
        /// 曲谱行
        /// </summary>
        public List<SheetMusicItem> Items { get; set; } = [];
    }

    /// <summary>
    /// 曲谱行集合
    /// </summary>
    internal class SheetMusic
    {
        /// <summary>
        /// 曲谱行集合
        /// </summary>
        public List<SheetMusicLine> Lines { get; set; } = [];
    }
}
