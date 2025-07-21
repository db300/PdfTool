using System;

namespace PdfEditor.Modules
{
    /// <summary>
    /// 浏览器预览器
    /// </summary>
    public partial class BrowserPreviewer : PageOperaBase
    {
        #region constructor
        public BrowserPreviewer()
        {
            InitializeComponent();
        }
        #endregion

        #region event handler
        protected override void PagePanel_PageSelect(object sender, EventArgs e, int pageNum)
        {
            base.PagePanel_PageSelect(sender, e, pageNum);
        }
        #endregion

        #region ui
        private void InitUi()
        {
        }
        #endregion
    }
}
