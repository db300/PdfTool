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
    /// PDF保护器
    /// </summary>
    public partial class PdfProtector : UserControl, IPdfHandler
    {
        #region constructor
        public PdfProtector()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtPwd;
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
                _txtLog.AppendText($"{fileName}\r\n");
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

        private void BtnProtect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtPwd.Text))
            {
                _txtLog.AppendText($"未输入保护密码\r\n");
                return;
            }
            var pwd = _txtPwd.Text.Trim();
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var fileName in _inputPdfFileList)
                {
                    var outputFileName = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}_protected.pdf");
                    var s = PdfHelperLibrary.ProtectHelper.Protect(fileName, outputFileName, pwd);
                    var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 保护完成，生成文件：{outputFileName}" : $"{fileName} {s}";
                    background.ReportProgress(0, msg);
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
                _txtLog.AppendText($"保护完成\r\n");
            };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在保护，请稍候...\r\n");
        }

        private void BtnUnprotect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtPwd.Text))
            {
                _txtLog.AppendText($"未输入密码\r\n");
                return;
            }
            var pwd = _txtPwd.Text.Trim();
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var fileName in _inputPdfFileList)
                {
                    var outputFileName = Path.Combine(Path.GetDirectoryName(fileName), $"{Path.GetFileNameWithoutExtension(fileName)}_unprotected.pdf");
                    var s = PdfHelperLibrary.ProtectHelper.Unprotect(fileName, outputFileName, pwd);
                    var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 解除保护完成，生成文件：{outputFileName}" : $"{fileName} {s}";
                    background.ReportProgress(0, msg);
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
                _txtLog.AppendText($"解除保护完成\r\n");
            };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在解除保护，请稍候...\r\n");
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

            _txtPwd = new TextBox
            {
                Location = new Point(btnAddFile.Right + Config.ControlPadding, Config.ControlMargin + 1),
                Parent = this,
            };

            var btnProtect = new Button
            {
                AutoSize = true,
                Location = new Point(_txtPwd.Right + Config.ControlPadding, Config.ControlMargin),
                Parent = this,
                Text = "加密"
            };
            btnProtect.Click += BtnProtect_Click;
            var btnUnprotect = new Button
            {
                AutoSize = true,
                Location = new Point(btnProtect.Right + Config.ControlPadding, Config.ControlMargin),
                Parent = this,
                Text = "解密"
            };
            btnUnprotect.Click += BtnUnprotect_Click;

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
