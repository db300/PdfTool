using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UkuleleEditor
{
    /// <summary>
    /// 曲谱实体
    /// </summary>
    internal class SheetMusicItem
    {
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
