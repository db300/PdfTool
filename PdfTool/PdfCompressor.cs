using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF压缩器
    /// </summary>
    public partial class PdfCompressor : UserControl
    {
        #region constructor
        public PdfCompressor()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtLog;
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

        private void BtnCompress_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要压缩的PDF文件\r\n";
                return;
            }
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var fileName in _inputPdfFileList)
                {
                    var outputFileName = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}_compressed.pdf");
                    var s = PdfHelperLibrary.CompressHelper.Compress(fileName, outputFileName);
                    var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 压缩完成，压缩前：{new FileInfo(fileName).Length}，压缩后：{new FileInfo(outputFileName).Length}" : $"{fileName} {s}";
                    background.ReportProgress(0, msg);
                }
            };
            background.ProgressChanged += (ww, ee) => { if (ee.UserState is string msg) { _txtLog.AppendText($"{msg}\r\n"); } };
            background.RunWorkerCompleted += (ww, ee) => { _txtLog.AppendText($"压缩完成\r\n"); };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在压缩，请稍候...\r\n");
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

            var btnCompress = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始压缩"
            };
            btnCompress.Click += BtnCompress_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - btnAddFile.Bottom - Config.ControlPadding),
                WordWrap = false
            };
        }
        #endregion
    }
}
