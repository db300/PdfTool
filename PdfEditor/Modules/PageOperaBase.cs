using PdfEditor.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PdfEditor.Modules
{
    /// <summary>
    /// 页面操作基类
    /// </summary>
    public partial class PageOperaBase : UserControl
    {
        #region constructor
        public PageOperaBase()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        protected string _inputPdfFileName;
        protected PdfHelperLibrary.ImagerHelper2 _helper;
        protected int _currentPageNum = -1;

        protected PagePanel _pagePanel;
        protected PictureBox _picBox;
        #endregion

        #region method
        public void OpenPdf(string fileName)
        {
            try
            {
                _inputPdfFileName = fileName;
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
                };
                background.RunWorkerCompleted += (ww, ee) => { };
                background.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region event handler
        protected virtual void PagePanel_PageSelect(object sender, EventArgs e, int pageNum)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Page {pageNum} selected.");
#endif
            _picBox.Image = _helper.GetPageImage(pageNum, 100);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            _pagePanel = new PagePanel
            {
                Dock = DockStyle.Left,
                Parent = this,
                Width = 200
            };
            _pagePanel.PageSelect += PagePanel_PageSelect;

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
