using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FileTool
{
    /// <summary>
    /// 完整匹配查找器(精确查找)
    /// </summary>
    public partial class ExactMatchFinder : UserControl
    {
        #region constructor
        public ExactMatchFinder()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private List<FileMoveInfo> _fileMoveInfos;
        private TextBox _txtSourceFolder;
        private TextBox _txtTargetFolder;
        private TextBox _txtSearchFileTable;
        private ListView _lvMoveTable;
        private TextBox _txtLog;
        private Button _btnSearch;
        #endregion

        #region method
        private void SearchFiles(DirectoryInfo sourceDirectory, BackgroundWorker background)
        {
            var allFiles = sourceDirectory.GetFiles("*", SearchOption.AllDirectories);
            var totalFiles = _fileMoveInfos.Count;
            var processedFiles = 0;
            foreach (var fileMoveInfo in _fileMoveInfos)
            {
                var matchedFile = allFiles.FirstOrDefault(file => file.Name.Equals(fileMoveInfo.FileName, StringComparison.OrdinalIgnoreCase));
                if (matchedFile != null)
                {
                    fileMoveInfo.FileInfo = matchedFile;
                }

                processedFiles++;
                background.ReportProgress(processedFiles, $"正在查找文件: {fileMoveInfo.FileName}");
            }
        }

        private void ShowSearchResult()
        {
            //将查找结果显示在listview中
            foreach (var fileMoveInfo in _fileMoveInfos)
            {
                var item = _lvMoveTable.Items.Cast<ListViewItem>().FirstOrDefault(a => a.Text.Equals(fileMoveInfo.FileName, StringComparison.OrdinalIgnoreCase));
                if (item == null) continue;
                item.SubItems.Add(fileMoveInfo.FileInfo?.FullName ?? "未找到");
            }
        }

        private void MoveOrCopy(bool isMove)
        {
            if (!(_fileMoveInfos?.Count > 0)) return;
            if (string.IsNullOrWhiteSpace(_txtTargetFolder.Text))
            {
                _txtLog.AppendText("未选择目标目录\r\n");
                return;
            }
            //处理_fileMoveInfos文件移动或复制操作
            var targetFolder = _txtTargetFolder.Text;
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ss, ee) =>
            {
                var count = 0;
                foreach (var fileMoveInfo in _fileMoveInfos)
                {
                    count++;
                    if (fileMoveInfo.FileInfo == null)
                    {
                        background.ReportProgress(count, $"未找到文件 {fileMoveInfo.FileName}");
                        continue;
                    }
                    var targetDir = string.IsNullOrWhiteSpace(fileMoveInfo.TargetFolder) ? targetFolder : Path.Combine(targetFolder, fileMoveInfo.TargetFolder);
                    if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
                    var targetFile = Path.Combine(targetDir, fileMoveInfo.FileInfo.Name);
                    if (File.Exists(targetFile))
                    {
                        background.ReportProgress(count, $"目标文件 {targetFile} 已存在，已略过...");
                        continue;
                    }
                    if (isMove)
                    {
                        File.Move(fileMoveInfo.FileInfo.FullName, targetFile);
                        background.ReportProgress(count, $"原文件 {fileMoveInfo.FileInfo.FullName} 已移动到 {targetFile}");
                    }
                    else
                    {
                        File.Copy(fileMoveInfo.FileInfo.FullName, targetFile);
                        background.ReportProgress(count, $"原文件 {fileMoveInfo.FileInfo.FullName} 已复制到 {targetFile}");
                    }
                }
            };
            background.ProgressChanged += (ss, ee) =>
            {
                _txtLog.AppendText($"【{ee.ProgressPercentage} / {_fileMoveInfos.Count}】{ee.UserState}\r\n");
            };
            background.RunWorkerCompleted += (ss, ee) =>
            {
                _txtLog.AppendText(isMove ? "移动完成\r\n" : "复制完成\r\n");
            };
            background.RunWorkerAsync();
        }
        #endregion

        #region event handler
        private void BtnAddSourceFolder_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog { Description = "选择查找目录", RootFolder = Environment.SpecialFolder.Desktop, SelectedPath = @"C:\", ShowNewFolderButton = false };
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            _txtSourceFolder.Text = folderDlg.SelectedPath;
        }

        private void BtnAddTargetFolder_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog { Description = "选择目标目录", RootFolder = Environment.SpecialFolder.Desktop, SelectedPath = @"C:\" };
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            _txtTargetFolder.Text = folderDlg.SelectedPath;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件(*.*)|*.*", Title = "选择查找表格" };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            _btnSearch.Enabled = false;
            _txtSearchFileTable.Text = openDlg.FileName;
            _txtLog.AppendText("正在读取表格...\r\n");
            var background = new BackgroundWorker();
            background.DoWork += (ss, ee) =>
            {
                var table = ExcelHelperLibrary.DataExtractHelper.Extract2Table(openDlg.FileName);
                ee.Result = table;
            };
            background.RunWorkerCompleted += (ss, ee) =>
            {
                if (!(ee.Result is List<List<string>> table))
                {
                    _txtLog.AppendText("表格读取失败\r\n");
                    return;
                }
                _fileMoveInfos = table.Select(a =>
                {
                    return a.Count < 2 ? new FileMoveInfo { FileName = a[0] } : new FileMoveInfo { FileName = a[0], TargetFolder = a[1] };
                }).ToList();
                _lvMoveTable.Items.Clear();
                foreach (var fileMoveInfo in _fileMoveInfos)
                {
                    var item = new ListViewItem(fileMoveInfo.FileName);
                    item.SubItems.Add(fileMoveInfo.TargetFolder);
                    _lvMoveTable.Items.Add(item);
                }
                _txtLog.AppendText("表格读取完成\r\n");
                _btnSearch.Enabled = true;
            };
            background.RunWorkerAsync();
            //_lvMoveTable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!(_fileMoveInfos?.Count > 0)) return;
            if (string.IsNullOrWhiteSpace(_txtSourceFolder.Text))
            {
                _txtLog.AppendText("未选择查找目录\r\n");
                return;
            }
            var sourceDirectory = new DirectoryInfo(_txtSourceFolder.Text);
            if (!sourceDirectory.Exists)
            {
                _txtLog.AppendText("查找目录不存在\r\n");
                return;
            }
            _txtLog.AppendText("正在查找文件...\r\n");
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ss, ee) =>
            {
                SearchFiles(sourceDirectory, background);
            };
            background.ProgressChanged += (ss, ee) =>
            {
                _txtLog.AppendText($"【{ee.ProgressPercentage} / {_fileMoveInfos.Count}】{ee.UserState}\r\n");
            };
            background.RunWorkerCompleted += (ss, ee) =>
            {
                ShowSearchResult();
                _txtLog.AppendText("查找完成\r\n");
            };
            background.RunWorkerAsync();
        }

        private void BtnMove_Click(object sender, EventArgs e)
        {
            MoveOrCopy(true);
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            MoveOrCopy(false);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var btnAddSourceFolder = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Text = "选择查找目录"
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
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddSourceFolder.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "选择目标目录"
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
            var btnImport = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddTargetFolder.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "导入查找表格"
            };
            btnImport.Click += BtnImport_Click;
            _txtSearchFileTable = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnImport.Right + Config.ControlPadding, btnImport.Top + 1),
                Parent = this,
                ReadOnly = true,
                Width = ClientSize.Width - Config.ControlMargin - btnImport.Right - Config.ControlPadding
            };
            _lvMoveTable = new ListView
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Columns =
                {
                    new ColumnHeader { Text = "文件名", Width = 200, TextAlign = HorizontalAlignment.Left },
                    new ColumnHeader { Text = "目标目录", Width = 200, TextAlign = HorizontalAlignment.Left },
                    new ColumnHeader { Text = "查找结果", Width = 200, TextAlign = HorizontalAlignment.Left }
                },
                FullRowSelect = true,
                GridLines = true,
                Location = new Point(Config.ControlMargin, btnImport.Bottom + Config.ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - 2 * Config.ControlMargin, 250),
                View = View.Details
            };
            _btnSearch = new Button
            {
                AutoSize = true,
                Enabled = false,
                Location = new Point(Config.ControlMargin, _lvMoveTable.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "开始检索"
            };
            _btnSearch.Click += BtnSearch_Click;
            var btnMove = new Button
            {
                AutoSize = true,
                Location = new Point(_btnSearch.Right + Config.ControlPadding, _btnSearch.Top),
                Parent = this,
                Text = "移动"
            };
            btnMove.Click += BtnMove_Click;
            var btnCopy = new Button
            {
                AutoSize = true,
                Location = new Point(btnMove.Right + Config.ControlPadding, _btnSearch.Top),
                Parent = this,
                Text = "复制"
            };
            btnCopy.Click += BtnCopy_Click;
            var top = _btnSearch.Bottom + Config.ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F, GraphicsUnit.Point),
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

    internal class FileMoveInfo
    {
        /// <summary>
        /// 查找文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 移动目标文件夹
        /// </summary>
        public string TargetFolder { get; set; }
        /// <summary>
        /// 查找到的文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }
    }
}
