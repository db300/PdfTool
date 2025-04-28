using System;
using System.Drawing;
using System.Windows.Forms;

namespace PdfEditor.Controls
{
    /// <summary>
    /// 页面预览面板(单页)
    /// </summary>
    public partial class PagePreviewPanel : UserControl
    {
        #region constructor
        public PagePreviewPanel()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private int _pageNum;
        private Label _lblPageNum;
        private PictureBox _picPage;
        #endregion

        #region method
        public void SetPage(int pageNum, Image img)
        {
            _pageNum = pageNum;
            _lblPageNum.Text = $"第 {pageNum + 1} 页";
            _picPage.Image = img;
        }
        #endregion

        #region ui
        private void InitUi()
        {
            _lblPageNum = new Label
            {
                Cursor = Cursors.Hand,
                Dock = DockStyle.Bottom,
                Parent = this,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _lblPageNum.Click += (sender, e) => PageSelect?.Invoke(sender, e, _pageNum);
            _picPage = new PictureBox
            {
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Parent = this,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            _picPage.BringToFront();
            _picPage.Click += (sender, e) => PageSelect?.Invoke(sender, e, _pageNum);
        }
        #endregion

        #region custom event
        public delegate void PageSelectHandler(object sender, EventArgs e, int pageNum);
        public event PageSelectHandler PageSelect;
        #endregion
    }
}
