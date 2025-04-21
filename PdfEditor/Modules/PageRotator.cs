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
        private string _inputPdfFileName;
        private PdfHelperLibrary.ImagerHelper2 _helper;
        private readonly Dictionary<int, int> _dict4Rotate = new Dictionary<int, int>();
        private int _currentPageNum = -1;
        private PagePanel _pagePanel;
        private PictureBox _picBox;
        #endregion

        #region method
        public void OpenPdf(string fileName)
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
        #endregion

        #region event handler
        private void PagePanel_PageSelect(object sender, EventArgs e, int pageNum)
        {
            _currentPageNum = pageNum;
            _picBox.Image = _dict4Rotate.ContainsKey(pageNum)
                ? _helper.GetPageImage(pageNum, 100, _dict4Rotate[_currentPageNum] * 90)
                : _helper.GetPageImage(pageNum, 100);
        }

        private void BtnRotate_Click(object sender, EventArgs e)
        {
            if (!(((Button)sender).Tag is int tag) || _currentPageNum == -1) return;
            if (_dict4Rotate.ContainsKey(_currentPageNum))
            {
                if (tag == 0) _dict4Rotate[_currentPageNum] = 0;
                else _dict4Rotate[_currentPageNum] += tag;
            }
            else
            {
                _dict4Rotate.Add(_currentPageNum, tag);
            }
            if (_dict4Rotate[_currentPageNum] > 3 || _dict4Rotate[_currentPageNum] < -3) _dict4Rotate[_currentPageNum] = 0;
            var img = _helper.GetPageImage(_currentPageNum, 100, _dict4Rotate[_currentPageNum] * 90);
            _picBox.Image = img;
            _pagePanel.UpdatePage(_currentPageNum, img);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            var saveDlg = new SaveFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*" };
            if (saveDlg.ShowDialog() != DialogResult.OK) return;
            var s = PdfHelperLibrary.PageRotateHelper.RotatePdf(_inputPdfFileName, saveDlg.FileName, _dict4Rotate);
            var msg = string.IsNullOrWhiteSpace(s) ? $"{_inputPdfFileName} 旋转完成: {saveDlg.FileName}" : $"{_inputPdfFileName} 旋转失败: {s}";
            MessageBox.Show(msg);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (Parent is TabPage tabPage)
            {
                tabPage.Dispose();
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Right,
                Parent = this,
                Width = 250
            };
            var btnRotateLeft = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = panel,
                Tag = -1,
                Text = "逆时针旋转"
            };
            btnRotateLeft.Click += BtnRotate_Click;
            var btnRotateRight = new Button
            {
                AutoSize = true,
                Location = new Point(btnRotateLeft.Right + Config.ControlPadding, Config.ControlMargin),
                Parent = panel,
                Tag = 1,
                Text = "顺时针旋转"
            };
            btnRotateRight.Click += BtnRotate_Click;
            var btnRestore = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnRotateLeft.Bottom + Config.ControlPadding),
                Parent = panel,
                Tag = 0,
                Text = "还原"
            };
            btnRestore.Click += BtnRotate_Click;

            var picLine = new PictureBox
            {
                BackColor = Color.LightGray,
                Location = new Point(Config.ControlMargin, btnRestore.Bottom + Config.ControlPadding),
                Parent = panel,
                Size = new Size(panel.ClientSize.Width - 2 * Config.ControlMargin, 1)
            };
            var btnExport = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, picLine.Bottom + Config.ControlPadding),
                Parent = panel,
                Text = "导出"
            };
            btnExport.Click += BtnExport_Click;
            var btnClose = new Button
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                AutoSize = true,
                Parent = panel,
                Text = "关闭文档"
            };
            btnClose.Location = new Point(Config.ControlMargin, panel.ClientSize.Height - Config.ControlMargin - btnClose.Height);
            btnClose.Click += BtnClose_Click;

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
