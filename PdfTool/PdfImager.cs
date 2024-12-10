using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF转图器
    /// </summary>
    public partial class PdfImager : UserControl
    {
        #region constructor
        public PdfImager()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtLog;
        private ComboBox _cmbDpi;
        private ComboBox _cmbFormat;
        private RadioButton _rbAllPage;
        private RadioButton _rbPartPage;
        private NumericUpDown _numPageFrom;
        private NumericUpDown _numPageTo;
        #endregion

        #region method
        public void OpenPdfs(List<string> files)
        {
            _txtLog.Clear();
            _inputPdfFileList.Clear();
            _inputPdfFileList.AddRange(files);
            foreach (var fileName in _inputPdfFileList)
            {
                _txtLog.AppendText($"【页数：{PdfHelperLibrary.CommonHelper.GetPageCount(fileName)}】{fileName}\r\n");
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenPdfs(openDlg.FileNames.ToList());
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要转换的PDF文件\r\n";
                return;
            }
            var dpi = int.Parse(_cmbDpi.Text);
            var format = _cmbFormat.Text;
            var allPage = _rbAllPage.Checked;
            var pageNums = allPage ? null : Enumerable.Range((int)_numPageFrom.Value, (int)_numPageTo.Value - (int)_numPageFrom.Value + 1).ToList();
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                if (allPage)
                {
                    foreach (var fileName in _inputPdfFileList)
                    {
                        var s = PdfHelperLibrary.ImagerHelper.ConvertPdfToImage(fileName, dpi, format, info => background.ReportProgress(0, info));
                        var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 转换完成" : $"{fileName} {s}";
                        background.ReportProgress(0, msg);
                    }
                }
                else
                {
                    foreach (var fileName in _inputPdfFileList)
                    {
                        var s = PdfHelperLibrary.ImagerHelper.ConvertPdfToImage(fileName, dpi, format, pageNums, info => background.ReportProgress(0, info));
                        var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 转换完成" : $"{fileName} {s}";
                        background.ReportProgress(0, msg);
                    }
                }
            };
            background.ProgressChanged += (ww, ee) =>
            {
                if (ee.UserState is string msg)
                {
                    _txtLog.AppendText($"{msg}\r\n");
                }
            };
            background.RunWorkerCompleted += (ww, ee) =>
            {
                _txtLog.AppendText($"转换完成\r\n");
            };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在转换，请稍候...\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var btnConvert = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始转换"
            };
            btnConvert.Click += BtnConvert_Click;

            var lbl = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "生成图片DPI："
            };
            _cmbDpi = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Parent = this
            };
            _cmbDpi.Items.AddRange(new object[] { 100, 200, 300, 600, 900, 1200 });
            _cmbDpi.SelectedIndex = 2;
            lbl = new Label
            {
                AutoSize = true,
                Location = new Point(_cmbDpi.Right + Config.ControlPadding, lbl.Top),
                Parent = this,
                Text = "生成图片格式："
            };
            _cmbFormat = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Parent = this
            };
            _cmbFormat.Items.AddRange(new object[] { "png", "jpg", "bmp" });
            _cmbFormat.SelectedIndex = 0;

            _rbAllPage = new RadioButton
            {
                AutoSize = true,
                Checked = true,
                Location = new Point(Config.ControlMargin, _cmbDpi.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "全部页面"
            };
            _rbPartPage = new RadioButton
            {
                AutoSize = true,
                Location = new Point(_rbAllPage.Right + Config.ControlPadding, _rbAllPage.Top),
                Parent = this,
                Text = "仅部分页面从："
            };
            _numPageFrom = new NumericUpDown
            {
                AutoSize = true,
                Maximum = 100000,
                Minimum = 1,
                Parent = this,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };
            _numPageFrom.Location = new Point(_rbPartPage.Right, _rbAllPage.Top + (_rbAllPage.Height - _numPageFrom.Height) / 2);
            lbl = new Label
            {
                AutoSize = true,
                Parent = this,
                Text = "到："
            };
            lbl.Location = new Point(_numPageFrom.Right + Config.ControlPadding, _rbAllPage.Top + (_rbAllPage.Height - lbl.Height) / 2);
            _numPageTo = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(lbl.Right, _numPageFrom.Top),
                Maximum = 100000,
                Minimum = 1,
                Parent = this,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };

            var top = _numPageFrom.Bottom + Config.ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, top),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - top),
                WordWrap = false
            };
        }
        #endregion
    }
}
