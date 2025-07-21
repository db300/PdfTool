using PdfEditor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PdfEditor.Modules
{
    /// <summary>
    /// 浏览器
    /// </summary>
    public partial class Browser : UserControl
    {
        #region constructor
        public Browser()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TreeView _tvPdf;
        private TextBox _txtFileInfo;
        private TabControl _tabPreviewer;

        private ContextMenuStrip _dirMenu;
        private ContextMenuStrip _pdfMenu;

        private readonly Dictionary<string, TabPage> _previewerDict = new Dictionary<string, TabPage>();
        #endregion

        #region method
        public void ScanPdf(string path)
        {
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                var allDirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
                allDirs.Insert(0, path); // 包含根目录

                var dirPdfMap = new Dictionary<string, List<string>>();
                foreach (var dir in allDirs)
                {
                    var pdfs = Directory.GetFiles(dir, "*.pdf", SearchOption.TopDirectoryOnly);
                    if (pdfs.Length > 0)
                        dirPdfMap[dir] = pdfs.ToList();
                }

#if DEBUG
                System.Diagnostics.Debug.WriteLine("PDF Directory Structure:");
                foreach (var kvp in dirPdfMap)
                {
                    System.Diagnostics.Debug.WriteLine($"Directory: {kvp.Key}");
                    foreach (var pdf in kvp.Value)
                    {
                        System.Diagnostics.Debug.WriteLine($"  - {pdf}");
                    }
                }
#endif

                ee.Result = dirPdfMap;
            };
            background.ProgressChanged += (ww, ee) =>
            {
            };
            background.RunWorkerCompleted += (ww, ee) =>
            {
                if (!(ee.Result is Dictionary<string, List<string>> dirPdfMap)) return;

                foreach (var item in dirPdfMap)
                {
                    var dirNode = new TreeNode(Path.GetFileName(item.Key)) { Tag = new NodeTagItem { Type = "DIR", Path = item.Key } };
                    _tvPdf.Nodes.Add(dirNode);
                    foreach (var pdf in item.Value)
                    {
                        var pdfNode = new TreeNode(Path.GetFileName(pdf)) { Tag = new NodeTagItem { Type = "PDF", Path = pdf } };
                        dirNode.Nodes.Add(pdfNode);
                    }
                }
            };
            background.RunWorkerAsync();
        }

        private void OpenDir()
        {
            if (_tvPdf.SelectedNode != null && _tvPdf.SelectedNode.Tag is NodeTagItem nodeTag && Directory.Exists(nodeTag.Path))
            {
                System.Diagnostics.Process.Start("explorer.exe", nodeTag.Path);
            }
        }

        private void OpenFileDir()
        {
            if (_tvPdf.SelectedNode != null && _tvPdf.SelectedNode.Tag is NodeTagItem nodeTag && File.Exists(nodeTag.Path))
            {
                string argument = $"/select,\"{nodeTag.Path}\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
        }

        private void ExportFileList()
        {
            if (_tvPdf.SelectedNode != null && _tvPdf.SelectedNode.Tag is NodeTagItem nodeTag && nodeTag.Type == "DIR")
            {
                using (var sfd = new SaveFileDialog { Title = "导出PDF文件列表", Filter = "CSV文件 (*.csv)|*.csv", FileName = $"{Path.GetFileName(nodeTag.Path)}_pdf_list.csv" })
                {
                    if (sfd.ShowDialog() != DialogResult.OK) return;
                    try
                    {
                        var pdfFiles = Directory.GetFiles(nodeTag.Path, "*.pdf");
                        var lines = new List<string>
                        {
                            // 添加表头
                            "文件名,文件路径,文件大小,创建时间,修改时间"
                        };
                        foreach (var file in pdfFiles)
                        {
                            var info = new FileInfo(file);
                            // 用双引号包裹，防止路径或文件名中有逗号
                            string line = $"\"{info.Name}\",\"{info.FullName}\",{info.Length},\"{info.CreationTime:yyyy-MM-dd HH:mm:ss}\",\"{info.LastWriteTime:yyyy-MM-dd HH:mm:ss}\"";
                            lines.Add(line);
                        }
                        File.WriteAllLines(sfd.FileName, lines, Encoding.UTF8);
                        MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"导出失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion

        #region event handler
        private void TvPdf_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _tvPdf.SelectedNode = e.Node; // 右键选中节点
                if (e.Node.Tag is NodeTagItem nodeTag)
                {
                    switch (nodeTag.Type)
                    {
                        case "DIR":
                            _dirMenu.Show(_tvPdf, e.Location);
                            break;
                        case "PDF":
                            _pdfMenu.Show(_tvPdf, e.Location);
                            break;
                    }
                }
            }
        }

        private void TvPdf_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is NodeTagItem nodeTag && nodeTag.Type == "PDF")
            {
                var path = nodeTag.Path;
                if (_previewerDict.ContainsKey(path))
                {
                    _tabPreviewer.SelectedTab = _previewerDict[path];
                }
                else
                {
                    try
                    {
                        var tabPage = new TabPage(Path.GetFileName(path));
                        var previewer = new BrowserPreviewer { Dock = DockStyle.Fill };
                        previewer.OpenPdf(path);
                        tabPage.Controls.Add(previewer);
                        _tabPreviewer.TabPages.Add(tabPage);
                        _tabPreviewer.SelectedTab = tabPage;
                        _previewerDict.Add(path, tabPage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{path} 打开失败，原因：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void TvPdf_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is NodeTagItem nodeTag)
            {
                switch (nodeTag.Type)
                {
                    case "DIR":
                        // 显示目录信息
                        var dirInfo = new DirectoryInfo(nodeTag.Path);
                        _txtFileInfo.Text = $"目录名: {dirInfo.Name}\r\n" +
                                            $"路径: {dirInfo.FullName}\r\n" +
                                            $"创建时间: {dirInfo.CreationTime}\r\n" +
                                            $"修改时间: {dirInfo.LastWriteTime}\r\n" +
                                            $"文件数: {dirInfo.GetFiles().Length}\r\n" +
                                            $"子目录数: {dirInfo.GetDirectories().Length}";
                        break;
                    case "PDF":
                        // 显示PDF文件信息
                        var fileInfo = new FileInfo(nodeTag.Path);
                        _txtFileInfo.Text = $"文件名: {fileInfo.Name}\r\n" +
                                            $"路径: {fileInfo.FullName}\r\n" +
                                            $"大小: {fileInfo.Length / 1024.0:F2} KB\r\n" +
                                            $"创建时间: {fileInfo.CreationTime}\r\n" +
                                            $"修改时间: {fileInfo.LastWriteTime}";
                        break;
                }
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            // 左侧Panel，包含TreeView和TextBox
            var leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250
            };
            Controls.Add(leftPanel);

            _tvPdf = new TreeView
            {
                Dock = DockStyle.Fill,
                HideSelection = false
            };
            _tvPdf.NodeMouseClick += TvPdf_NodeMouseClick;
            _tvPdf.NodeMouseDoubleClick += TvPdf_NodeMouseDoubleClick;
            _tvPdf.AfterSelect += TvPdf_AfterSelect;
            leftPanel.Controls.Add(_tvPdf);

            _txtFileInfo = new TextBox
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };
            leftPanel.Controls.Add(_txtFileInfo);

            _tabPreviewer = new TabControl
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(_tabPreviewer);
            _tabPreviewer.BringToFront();

            _dirMenu = new ContextMenuStrip();
            //_dirMenu.Items.Add("刷新目录", null, (s, e) => { });
            _dirMenu.Items.Add("打开文件夹", null, (s, e) => { OpenDir(); });
            _dirMenu.Items.Add("导出文件列表", null, (s, e) => { ExportFileList(); });

            _pdfMenu = new ContextMenuStrip();
            _pdfMenu.Items.Add("打开文件所在目录", null, (s, e) => { OpenFileDir(); });
            //_pdfMenu.Items.Add("删除文件", null, (s, e) => { });
        }
        #endregion
    }
}
