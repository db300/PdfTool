using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfEditor.Controls
{
    /// <summary>
    /// 页面预览面板
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
        private Label _lblPageNum;
        private PictureBox _picPage;
        #endregion

        #region ui
        private void InitUi()
        {
            _lblPageNum = new Label
            {
                Dock = DockStyle.Bottom,
                Parent = this,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _picPage = new PictureBox
            {
                Dock = DockStyle.Fill,
                Parent = this,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            _picPage.BringToFront();
        }
        #endregion
    }
}
