using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PdfEditor.Controls;

namespace PdfEditor.Modules
{
    /// <summary>
    /// 页面旋转器
    /// </summary>
    public partial class PageRotator : UserControl
    {
        #region constructor
        public PageRotator()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private PdfHelperLibrary.ImagerHelper2 _helper;
        private Stream _stream;
        private PagePanel _pagePanel;
        private PictureBox _picBox;
        #endregion

        #region method
        public void OpenPdf(string fileName)
        {
            _helper = new PdfHelperLibrary.ImagerHelper2(fileName);
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                var pageCount = _helper.PageCount;
                for (var i = 0; i < pageCount; i++)
                {
                    var img = _helper.GetPageImage(i, 100);
                    background.ReportProgress(i, img);
                }
            };
            background.ProgressChanged += (ww, ee) =>
            {
                var pageNum = ee.ProgressPercentage;
                var img = (Image)ee.UserState;
                _pagePanel.AddPage(pageNum, img);
                /*
                var panel = new PagePreviewPanel { Dock = DockStyle.Top };
                panel.SetPage(ee.ProgressPercentage, img);
                panel.PageSelect += Panel_PageSelect;
                _flPanel.Controls.Add(panel);
                */
            };
            background.RunWorkerCompleted += (ww, ee) => { };
            background.RunWorkerAsync();
        }
        #endregion

        #region event handler
        private void Panel_PageSelect(object sender, EventArgs e, int pageNum)
        {
            _picBox.Image = _helper.GetPageImage(pageNum, 100);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Right,
                Parent = this,
                Width = 200
            };
            var btnRotateLeft = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = panel,
                Text = "逆时针旋转"
            };
            var btnRotateRight = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnRotateLeft.Bottom + Config.ControlPadding),
                Parent = panel,
                Text = "顺时针旋转"
            };

            _pagePanel = new PagePanel
            {
                Dock = DockStyle.Left,
                Parent = this,
                Width = 200
            };

            _picBox = new PictureBox
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Parent = this,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            _picBox.BringToFront();
        }
        #endregion
    }
}
