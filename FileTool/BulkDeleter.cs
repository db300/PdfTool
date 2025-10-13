using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTool
{
    /// <summary>
    /// 批量删除器
    /// </summary>
    public partial class BulkDeleter : UserControl
    {
        #region constructor
        public BulkDeleter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private const string TxtTips = "输入要删除的准确文件夹名称";
        private string _rootDir;
        private CheckBox _ckbContainSubDir;
        private TextBox _txtDirFullName;
        private DateTimePicker _dtSpecialDate;

        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnSelectRootDir_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            _rootDir = folderDlg.SelectedPath;
            _txtLog.AppendText($"选择的根目录：{_rootDir}\r\n");
        }

        private async void BtnDeleteSpecialDir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_rootDir) || !Directory.Exists(_rootDir))
            {
                _txtLog.AppendText("未选择或根目录不存在\r\n");
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtDirFullName.Text) || _txtDirFullName.Text == TxtTips)
            {
                _txtLog.AppendText("未输入要删除的文件夹名称\r\n");
                return;
            }

            _txtLog.AppendText("正在检索目录，请稍候...\r\n");

            var progress = new Progress<string>(msg => _txtLog.AppendText(msg));
            List<string> dirList = null;

            try
            {
                dirList = await Task.Run(() =>
                {
                    var list = new List<string>();
                    try
                    {
                        var option = _ckbContainSubDir.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                        list.AddRange(Directory.GetDirectories(_rootDir, _txtDirFullName.Text, option));
                    }
                    catch (Exception ex)
                    {
                        (progress as IProgress<string>)?.Report($"检索目录失败：{ex.Message}\r\n");
                    }
                    return list;
                });
            }
            catch (Exception ex)
            {
                _txtLog.AppendText($"获取目录失败：{ex.Message}\r\n");
                return;
            }

            if (!(dirList?.Count > 0))
            {
                _txtLog.AppendText("未找到匹配的文件夹\r\n");
                return;
            }

            _txtLog.AppendText($"共找到 {dirList.Count} 个待删除文件夹，开始删除...\r\n");

            var successCount = 0;
            await Task.Run(() =>
            {
                int total = dirList.Count;
                int current = 0;
                foreach (var dir in dirList)
                {
                    current++;
                    try
                    {
                        Directory.Delete(dir, true);
                        successCount++;
                        (progress as IProgress<string>)?.Report($"[{current}/{total}] 已删除：{dir}\r\n");
                    }
                    catch (Exception ex)
                    {
                        (progress as IProgress<string>)?.Report($"[{current}/{total}] 删除失败：{dir}，原因：{ex.Message}\r\n");
                    }
                }
            });

            _txtLog.AppendText($"操作完成，共找到 {dirList.Count} 个文件夹，成功删除 {successCount} 个\r\n");
        }

        private async void BtnDeleteAllSubDir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除根目录下的所有子目录吗？此操作不可撤销！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_rootDir) || !Directory.Exists(_rootDir))
            {
                _txtLog.AppendText("未选择或根目录不存在\r\n");
                return;
            }

            _txtLog.AppendText("正在检索根目录下的所有子目录，请稍候...\r\n");

            var progress = new Progress<string>(msg => _txtLog.AppendText(msg));
            List<string> subDirs = null;

            try
            {
                subDirs = await Task.Run(() =>
                {
                    var list = new List<string>();
                    try
                    {
                        list.AddRange(Directory.GetDirectories(_rootDir, "*", SearchOption.TopDirectoryOnly));
                    }
                    catch (Exception ex)
                    {
                        (progress as IProgress<string>)?.Report($"检索子目录失败：{ex.Message}\r\n");
                    }
                    return list;
                });
            }
            catch (Exception ex)
            {
                _txtLog.AppendText($"获取子目录失败：{ex.Message}\r\n");
                return;
            }

            if (!(subDirs?.Count > 0))
            {
                _txtLog.AppendText("根目录下未找到任何子目录\r\n");
                return;
            }

            if (MessageBox.Show($"共找到 {subDirs.Count} 个子目录，确定要删除吗？此操作不可撤销！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }

            _txtLog.AppendText($"共找到 {subDirs.Count} 个子目录，开始删除...\r\n");

            var successCount = 0;
            await Task.Run(() =>
            {
                int total = subDirs.Count;
                int current = 0;
                foreach (var dir in subDirs)
                {
                    current++;
                    try
                    {
                        Directory.Delete(dir, true);
                        successCount++;
                        (progress as IProgress<string>)?.Report($"[{current}/{total}] 已删除：{dir}\r\n");
                    }
                    catch (Exception ex)
                    {
                        (progress as IProgress<string>)?.Report($"[{current}/{total}] 删除失败：{dir}，原因：{ex.Message}\r\n");
                    }
                }
            });

            _txtLog.AppendText($"操作完成，共找到 {subDirs.Count} 个子目录，成功删除 {successCount} 个\r\n");
        }

        private async void BtnDeleteSpecialDate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_rootDir) || !Directory.Exists(_rootDir))
            {
                _txtLog.AppendText("未选择或根目录不存在\r\n");
                return;
            }
            var targetDate = _dtSpecialDate.Value.Date;
            if (MessageBox.Show($"确定要删除 {_rootDir} 目录下，{targetDate:yyyy-MM-dd} 之前的所有文件吗？此操作不可撤销！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }
            _txtLog.AppendText("正在检索文件，请稍候...\r\n");
            var progress = new Progress<string>(msg => _txtLog.AppendText(msg));
            List<string> fileList = null;
            try
            {
                fileList = await Task.Run(() =>
                {
                    var list = new List<string>();
                    try
                    {
                        list.AddRange(Directory.GetFiles(_rootDir, "*.*", SearchOption.AllDirectories).Where(f => File.GetLastWriteTime(f).Date < targetDate));
                    }
                    catch (Exception ex)
                    {
                        (progress as IProgress<string>)?.Report($"检索文件失败：{ex.Message}\r\n");
                    }
                    return list;
                });
            }
            catch (Exception ex)
            {
                _txtLog.AppendText($"获取文件失败：{ex.Message}\r\n");
                return;
            }
            if (!(fileList?.Count > 0))
            {
                _txtLog.AppendText("未找到匹配的文件\r\n");
                return;
            }
            if (MessageBox.Show($"共找到 {fileList.Count} 个文件，确定要删除吗？此操作不可撤销！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                return;
            }
            _txtLog.AppendText($"共找到 {fileList.Count} 个待删除文件，开始删除...\r\n");
            var successCount = 0;
            await Task.Run(() =>
            {
                int total = fileList.Count;
                int current = 0;
                foreach (var file in fileList)
                {
                    current++;
                    try
                    {
                        File.Delete(file);
                        successCount++;
                        (progress as IProgress<string>)?.Report($"[{current}/{total}] 已删除：{file}\r\n");
                    }
                    catch (Exception ex)
                    {
                        (progress as IProgress<string>)?.Report($"[{current}/{total}] 删除失败：{file}，原因：{ex.Message}\r\n");
                    }
                }
            });
            _txtLog.AppendText($"操作完成，共找到 {fileList.Count} 个文件，成功删除 {successCount} 个\r\n");
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
                Text = "文件删除"
            };
            InitUi4FileGroup(group4File);
            var group4Dir = new GroupBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Config.ControlMargin, group4File.Bottom + Config.ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 140),
                Text = "文件夹删除"
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
            var btnSelectRootDir = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin + Config.ControlPadding),
                Parent = groupBox,
                Text = "选择根目录"
            };
            btnSelectRootDir.Click += BtnSelectRootDir_Click;

            var btnDeleteSpecialDate = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnSelectRootDir.Bottom + Config.ControlPadding),
                Parent = groupBox,
                Text = "批量删除指定日期之前的文件"
            };
            btnDeleteSpecialDate.Click += BtnDeleteSpecialDate_Click;
            _dtSpecialDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                //ShowUpDown = true,
                Location = new Point(btnDeleteSpecialDate.Right + Config.ControlPadding, btnDeleteSpecialDate.Top + 1),
                Parent = groupBox,
                Width = 120,
                Value = DateTime.Now.AddDays(-30)
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
                Checked = true,
                Location = new Point(btnSelectRootDir.Right + Config.ControlPadding, btnSelectRootDir.Top + 3),
                Parent = groupBox,
                Text = "包含子目录"
            };

            var btnDeleteSpecialDir = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnSelectRootDir.Bottom + Config.ControlPadding),
                Parent = groupBox,
                Text = "批量删除指定目录"
            };
            btnDeleteSpecialDir.Click += BtnDeleteSpecialDir_Click;
            _txtDirFullName = new TextBox
            {
                Location = new Point(btnDeleteSpecialDir.Right + Config.ControlPadding, btnDeleteSpecialDir.Top + 1),
                Parent = groupBox,
                Width = 250,
                Text = TxtTips
            };

            var btnDeleteAllSubDir = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnDeleteSpecialDir.Bottom + Config.ControlPadding),
                Parent = groupBox,
                Text = "删除根目录下所有子目录"
            };
            btnDeleteAllSubDir.Click += BtnDeleteAllSubDir_Click;
        }
        #endregion
    }
}
