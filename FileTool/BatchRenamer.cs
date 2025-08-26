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
    /// 批量改名
    /// </summary>
    public partial class BatchRenamer : UserControl
    {
        #region constructor
        public BatchRenamer()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private TextBox _txtDesExt;

        private string _rootDir;
        private CheckBox _ckbContainSubDir;
        private TextBox _txtDirNamePattern;
        private TextBox _txtDirNameReplacement;

        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnAddFiles_Click(object sender, EventArgs e)
        {
            var fileDlg = new OpenFileDialog { Multiselect = true, Title = "选择文件", Filter = "所有文件|*.*" };
            if (fileDlg.ShowDialog() != DialogResult.OK) return;
            _inputFileList.Clear();
            _inputFileList.AddRange(fileDlg.FileNames);
            _txtLog.AppendText($"已添加{_inputFileList.Count}个文件\r\n");
        }

        private void BtnRenameExt_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                _txtLog.AppendText("未添加文件\r\n");
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtDesExt.Text))
            {
                _txtLog.AppendText("未输入目标扩展名\r\n");
                return;
            }
            var desExt = _txtDesExt.Text;
            if (!desExt.StartsWith(".")) desExt = $".{desExt}";
            var successCount = 0;
            foreach (var file in _inputFileList)
            {
                var desFile = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}{desExt}");
                try
                {
                    File.Move(file, desFile);
                    successCount++;
                    _txtLog.AppendText($"重命名成功：{file} => {desFile}\r\n");
                }
                catch (Exception ex)
                {
                    _txtLog.AppendText($"重命名失败：{file} => {desFile}，{ex.Message}\r\n");
                }
            }
            _txtLog.AppendText($"共{_inputFileList.Count}个文件，成功{successCount}个\r\n");
        }

        private void BtnSelectRootDir_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            _rootDir = folderDlg.SelectedPath;
            _txtLog.AppendText($"选择的根目录：{_rootDir}\r\n");
        }

        private void BtnReplaceDirName_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_rootDir) || !Directory.Exists(_rootDir))
            {
                _txtLog.AppendText("未选择根目录或目录不存在\r\n");
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtDirNamePattern.Text))
            {
                _txtLog.AppendText("未输入目录名匹配模式\r\n");
                return;
            }
            var pattern = _txtDirNamePattern.Text;
            var replacement = _txtDirNameReplacement.Text ?? string.Empty;
            var dirList = _ckbContainSubDir.Checked
                ? Directory.GetDirectories(_rootDir, "*", SearchOption.AllDirectories).ToList()
                : Directory.GetDirectories(_rootDir, "*", SearchOption.TopDirectoryOnly).ToList();
            var successCount = 0;
            foreach (var dir in dirList)
            {
                var dirName = Path.GetFileName(dir);
                if (!dirName.Contains(pattern)) continue;
                var newDirName = dirName.Replace(pattern, replacement);
                var newDir = Path.Combine(Path.GetDirectoryName(dir), newDirName);
                try
                {
                    Directory.Move(dir, newDir);
                    successCount++;
                    _txtLog.AppendText($"重命名成功：{dir} => {newDir}\r\n");
                }
                catch (Exception ex)
                {
                    _txtLog.AppendText($"重命名失败：{dir} => {newDir}，{ex.Message}\r\n");
                }
            }
            _txtLog.AppendText($"处理完成，共 {dirList.Count} 个目录，成功 {successCount} 个\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var group4File = new GroupBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 110),
                Text = "文件改名"
            };
            InitUi4FileGroup(group4File);
            var group4Dir = new GroupBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Config.ControlMargin, group4File.Bottom + Config.ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 110),
                Text = "文件夹改名"
            };
            InitUi4DirGroup(group4Dir);

            var top = group4Dir.Bottom + Config.ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F),
                Location = new Point(Config.ControlMargin, top),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - top)
            };
        }

        private void InitUi4FileGroup(GroupBox groupBox)
        {
            var btnAddFiles = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin + Config.ControlPadding),
                Parent = groupBox,
                Text = "添加文件"
            };
            btnAddFiles.Click += BtnAddFiles_Click;

            var btnRenameExt = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddFiles.Bottom + Config.ControlPadding),
                Parent = groupBox,
                Text = "批量修改扩展名"
            };
            btnRenameExt.Click += BtnRenameExt_Click;
            _txtDesExt = new TextBox
            {
                Location = new Point(btnRenameExt.Right + Config.ControlPadding, btnRenameExt.Top + 1),
                Parent = groupBox,
                Text = ".xxx"
            };
        }

        private void InitUi4DirGroup(GroupBox groupBox)
        {
            var btnSelectRootDir = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin + Config.ControlPadding),
                Parent = groupBox,
                Text = "选择根目录"
            };
            btnSelectRootDir.Click += BtnSelectRootDir_Click;

            _ckbContainSubDir = new CheckBox
            {
                AutoSize = true,
                Location = new Point(btnSelectRootDir.Right + Config.ControlPadding, btnSelectRootDir.Top + 3),
                Parent = groupBox,
                Text = "包含子目录"
            };

            var btnReplaceDirName = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnSelectRootDir.Bottom + Config.ControlPadding),
                Parent = groupBox,
                Text = "批量替换目录名"
            };
            btnReplaceDirName.Click += BtnReplaceDirName_Click;
            _txtDirNamePattern = new TextBox
            {
                Location = new Point(btnReplaceDirName.Right + Config.ControlPadding, btnReplaceDirName.Top + 1),
                Parent = groupBox,
                Width = 150,
                Text = "旧"
            };
            _txtDirNameReplacement = new TextBox
            {
                Location = new Point(_txtDirNamePattern.Right + Config.ControlPadding, btnReplaceDirName.Top + 1),
                Parent = groupBox,
                Width = 150,
                Text = "新"
            };
        }
        #endregion
    }
}
