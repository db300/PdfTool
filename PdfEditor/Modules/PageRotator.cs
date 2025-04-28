using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
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

        private void Rotate(int pageNum, int angleTag)
        {
            if (_dict4Rotate.ContainsKey(pageNum))
            {
                if (angleTag == 0) _dict4Rotate[pageNum] = 0;
                else _dict4Rotate[pageNum] += angleTag;
            }
            else
            {
                _dict4Rotate.Add(pageNum, angleTag);
            }
            if (_dict4Rotate[pageNum] > 3 || _dict4Rotate[pageNum] < -3) _dict4Rotate[pageNum] = 0;
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
            Rotate(_currentPageNum, tag);
            var img = _helper.GetPageImage(_currentPageNum, 100, _dict4Rotate[_currentPageNum] * 90);
            _picBox.Image = img;
            _pagePanel.UpdatePage(_currentPageNum, img);
        }

        private void BtnAllRotate_Click(object sender, EventArgs e)
        {
            if (!(((Button)sender).Tag is int tag)) return;
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                for (var i = 0; i < _helper.PageCount; i++)
                {
                    Rotate(i, tag);
                    var img = _helper.GetPageImage(i, 100, _dict4Rotate[i] * 90);
                    background.ReportProgress(i, img);
                }
            };
            background.ProgressChanged += (ww, ee) =>
            {
                if (!(ee.UserState is Bitmap img)) return;
                _pagePanel.UpdatePage(ee.ProgressPercentage, img);
                if (_currentPageNum == ee.ProgressPercentage) _picBox.Image = img;
            };
            background.RunWorkerCompleted += (ww, ee) =>
            {
                ((Button)sender).Enabled = true;
            };
            background.RunWorkerAsync();
            ((Button)sender).Enabled = false;
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

            //单页旋转
            var lbl = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = panel,
                Text = "单页旋转"
            };
            //lbl.Location = new Point((panel.ClientSize.Width - lbl.Width) / 2, Config.ControlMargin);
            var btnRotateLeft = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, lbl.Bottom + Config.ControlPadding),
                Parent = panel,
                Tag = -1,
                Text = "逆时针旋转"
            };
            btnRotateLeft.Click += BtnRotate_Click;
            var btnRotateRight = new Button
            {
                AutoSize = true,
                Location = new Point(btnRotateLeft.Right + Config.ControlPadding, btnRotateLeft.Top),
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

            //分隔线
            var picLine = new PictureBox
            {
                BackColor = Color.LightGray,
                Location = new Point(Config.ControlMargin, btnRestore.Bottom + Config.ControlPadding),
                Parent = panel,
                Size = new Size(panel.ClientSize.Width - 2 * Config.ControlMargin, 1)
            };

            //全部旋转
            lbl = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, picLine.Bottom + Config.ControlPadding),
                Parent = panel,
                Text = "全部旋转"
            };
            //lbl.Location = new Point((panel.ClientSize.Width - lbl.Width) / 2, picLine.Bottom + Config.ControlPadding);
            var btnAllRotateLeft = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, lbl.Bottom + Config.ControlPadding),
                Parent = panel,
                Tag = -1,
                Text = "逆时针旋转"
            };
            btnAllRotateLeft.Click += BtnAllRotate_Click;
            var btnAllRotateRight = new Button
            {
                AutoSize = true,
                Location = new Point(btnAllRotateLeft.Right + Config.ControlPadding, btnAllRotateLeft.Top),
                Parent = panel,
                Tag = 1,
                Text = "顺时针旋转"
            };
            btnAllRotateRight.Click += BtnAllRotate_Click;
            var btnAllRestore = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAllRotateLeft.Bottom + Config.ControlPadding),
                Parent = panel,
                Tag = 0,
                Text = "还原"
            };
            btnAllRestore.Click += BtnAllRotate_Click;

            //分隔线
            picLine = new PictureBox
            {
                BackColor = Color.LightGray,
                Location = new Point(Config.ControlMargin, btnAllRestore.Bottom + Config.ControlPadding),
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
