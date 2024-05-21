using System.Collections.Generic;

namespace PdfDesignHelper
{
    internal class CellItem
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public float X { get; set; }
        public float Y { get; set; }
        public float W { get; set; }
        public float H { get; set; }

        /// <summary>
        /// 字号
        /// </summary>
        public float EmSize { get; set; }
    }

    internal class CellList : List<CellItem>
    {
    }
}
