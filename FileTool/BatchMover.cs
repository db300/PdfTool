using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTool
{
    /// <summary>
    /// 批量移动
    /// </summary>
    public partial class BatchMover : UserControl
    {
        #region constructor
        public BatchMover()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TextBox _txtSourceFolder;
        private TextBox _txtTargetFolder;
        private TextBox _txtSearchPattern;
        private CheckBox _ckbIgnoreExistsFiles;
        private CheckBox _ckbKeepSourceFiles;
        private CheckBox _ckbCheckRootOnly;
        private Button _btnMove;
        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnAddSourceFolder_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog { Description = "选择源文件夹", RootFolder = Environment.SpecialFolder.Desktop, SelectedPath = @"C:\", ShowNewFolderButton = false };
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            _txtSourceFolder.Text = folderDlg.SelectedPath;
        }

        private void BtnAddTargetFolder_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog { Description = "选择目标文件夹", RootFolder = Environment.SpecialFolder.Desktop, SelectedPath = @"C:\" };
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            _txtTargetFolder.Text = folderDlg.SelectedPath;
        }

        private void BtnMove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtSourceFolder.Text))
            {
                _txtLog.Text = "未选择源文件夹\r\n";
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtTargetFolder.Text))
            {
                _txtLog.Text = "未选择目标文件夹\r\n";
                return;
            }
            var ignoreExistsFiles = _ckbIgnoreExistsFiles.Checked;
            var keepSrcFiles = _ckbKeepSourceFiles.Checked;
            var sourceFolder = _txtSourceFolder.Text;
            var targetFolder = _txtTargetFolder.Text;
            var files = Directory.GetFiles(sourceFolder, _txtSearchPattern.Text.Trim(), _ckbCheckRootOnly.Checked ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
            var total = files.Length;
            _txtLog.Text = keepSrcFiles ? "开始复制文件...\r\n" : "开始移动文件...\r\n";
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                var count = 0;
                foreach (var file in files)
                {
                    count++;
                    var targetFile = Path.Combine(targetFolder, Path.GetFileName(file));
                    if (File.Exists(targetFile))
                    {
                        if (ignoreExistsFiles)
                        {
                            background.ReportProgress(count, $"目标文件 {targetFile} 已存在，已略过...");
                            continue;
                        }
                        File.Delete(targetFile);
                        background.ReportProgress(count, $"目标文件 {targetFile} 已存在，进行覆盖...");
                    }
                    if (_ckbKeepSourceFiles.Checked)
                    {
                        File.Copy(file, targetFile);
                        background.ReportProgress(count, $"原文件 {file} 已复制到 {targetFile}");
                    }
                    else
                    {
                        File.Move(file, targetFile);
                        background.ReportProgress(count, $"原文件 {file} 已移动到 {targetFile}");
                    }
                }
            };
            background.ProgressChanged += (ss, ee) =>
            {
                _txtLog.AppendText($"【{ee.ProgressPercentage} / {total}】{ee.UserState}\r\n");
            };
            background.RunWorkerCompleted += (ss, ee) =>
            {
                _txtLog.AppendText(keepSrcFiles ? "复制完成\r\n" : "移动完成\r\n");
            };
            background.RunWorkerAsync();
        }

        private void CkbKeepSourceFiles_CheckedChanged(object sender, EventArgs e)
        {
            _btnMove.Text = ((CheckBox)sender).Checked ? "开始复制" : "开始移动";
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var btnAddSourceFolder = new Button
            {
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Text = "选择源文件夹",
                Width = 100
            };
            btnAddSourceFolder.Click += BtnAddSourceFolder_Click;
            _txtSourceFolder = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnAddSourceFolder.Right + Config.ControlPadding, btnAddSourceFolder.Top + 1),
                Parent = this,
                ReadOnly = true,
                Width = ClientSize.Width - Config.ControlMargin - btnAddSourceFolder.Right - Config.ControlPadding
            };
            var btnAddTargetFolder = new Button
            {
                Location = new Point(Config.ControlMargin, btnAddSourceFolder.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "选择目标文件夹",
                Width = 100
            };
            btnAddTargetFolder.Click += BtnAddTargetFolder_Click;
            _txtTargetFolder = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnAddTargetFolder.Right + Config.ControlPadding, btnAddTargetFolder.Top + 1),
                Parent = this,
                ReadOnly = true,
                Width = ClientSize.Width - Config.ControlMargin - btnAddTargetFolder.Right - Config.ControlPadding
            };
            _txtSearchPattern = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Location = new Point(_txtTargetFolder.Left, _txtTargetFolder.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "*.*",
                Width = 200
            };
            var lbl = new Label
            {
                AutoSize = true,
                Parent = this,
                Text = "文件名匹配："
            };
            lbl.Location = new Point(_txtSearchPattern.Left - lbl.Width, _txtSearchPattern.Top + 3);
            _ckbIgnoreExistsFiles = new CheckBox
            {
                AutoSize = true,
                Location = new Point(_txtSearchPattern.Right + Config.ControlPadding, _txtSearchPattern.Top + 2),
                Parent = this,
                Text = "忽略已存在文件"
            };
            _ckbKeepSourceFiles = new CheckBox
            {
                AutoSize = true,
                Location = new Point(_ckbIgnoreExistsFiles.Right + Config.ControlPadding, _ckbIgnoreExistsFiles.Top),
                Parent = this,
                Text = "保留源文件"
            };
            _ckbKeepSourceFiles.CheckedChanged += CkbKeepSourceFiles_CheckedChanged;
            _ckbCheckRootOnly = new CheckBox
            {
                AutoSize = true,
                Location = new Point(_ckbKeepSourceFiles.Right + Config.ControlPadding, _ckbIgnoreExistsFiles.Top),
                Parent = this,
                Text = "只检索源文件夹根目录"
            };
            _btnMove = new Button
            {
                Location = new Point(Config.ControlMargin, _ckbKeepSourceFiles.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "开始移动",
                Width = 100
            };
            _btnMove.Click += BtnMove_Click;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F, GraphicsUnit.Point),
                Location = new Point(Config.ControlMargin, _btnMove.Bottom + Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - _btnMove.Bottom - Config.ControlPadding),
                WordWrap = false
            };
        }
        #endregion
    }
}
