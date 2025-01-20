using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// 页面旋转器
    /// </summary>
    public partial class PageRotator : UserControl, IPdfHandler
    {
        #region constructor
        public PageRotator()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtLog;
        private ComboBox _cmbRotationAngle;
        private RadioButton _rbAllPage;
        private RadioButton _rbPartPage;
        private TextBox _txtPageNums;
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

        private List<int> ParsePageNumbers(string pageNumbersText)
        {
            var pageNums = new List<int>();
            var parts = pageNumbersText.Split(new[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.Contains('-'))
                {
                    var rangeParts = part.Split('-');
                    if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out var start) && int.TryParse(rangeParts[1], out var end))
                    {
                        for (var i = start; i <= end; i++)
                        {
                            pageNums.Add(i);
                        }
                    }
                }
                else if (int.TryParse(part, out var pageNum))
                {
                    pageNums.Add(pageNum);
                }
            }

            return pageNums;
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenPdfs(openDlg.FileNames.ToList());
        }

        private void BtnRotate_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.AppendText("未添加需要旋转的PDF文件\r\n");
                return;
            }
            var allPage = _rbAllPage.Checked;
            var rotateAngle = int.Parse(_cmbRotationAngle.Text);
            var pageNums = allPage ? null : ParsePageNumbers(_txtPageNums.Text);
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                if (allPage)
                {
                    foreach (var fileName in _inputPdfFileList)
                    {
                        var s = PdfHelperLibrary.PageRotateHelper.RotatePdf(fileName, out var outputFileName, rotateAngle);
                        var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 旋转完成: {outputFileName}" : $"{fileName} 旋转失败: {s}";
                        background.ReportProgress(0, msg);
                    }
                }
                else
                {
                    foreach (var fileName in _inputPdfFileList)
                    {
                        var s = PdfHelperLibrary.PageRotateHelper.RotatePdf(fileName, out var outputFileName, rotateAngle, pageNums);
                        var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 旋转完成: {outputFileName}" : $"{fileName} 旋转失败: {s}";
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
                _txtLog.AppendText($"旋转完成\r\n");
            };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在旋转，请稍候...\r\n");
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
            var btnRotate = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始旋转"
            };
            btnRotate.Click += BtnRotate_Click;

            var lbl = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "旋转角度："
            };
            _cmbRotationAngle = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Parent = this
            };
            _cmbRotationAngle.Items.AddRange(new object[] { 90, 180, 270 });
            _cmbRotationAngle.SelectedIndex = 0;

            _rbAllPage = new RadioButton
            {
                AutoSize = true,
                Checked = true,
                Location = new Point(Config.ControlMargin, _cmbRotationAngle.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "全部页面"
            };
            _rbPartPage = new RadioButton
            {
                AutoSize = true,
                Location = new Point(_rbAllPage.Right + Config.ControlPadding, _rbAllPage.Top),
                Parent = this,
                Text = "仅部分页面："
            };
            _txtPageNums = new TextBox
            {

                Parent = this,
                Width = 200
            };
            _txtPageNums.Location = new Point(_rbPartPage.Right, _rbAllPage.Top + (_rbAllPage.Height - _txtPageNums.Height) / 2);
            lbl = new Label
            {
                AutoSize = true,
                ForeColor = Color.Blue,
                Parent = this,
                Text = "（例如：1,3,5-9）"
            };
            lbl.Location = new Point(_txtPageNums.Right, _rbAllPage.Top + (_rbAllPage.Height - lbl.Height) / 2);

            var top = _txtPageNums.Bottom + Config.ControlPadding;
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
