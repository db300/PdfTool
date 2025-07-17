using PdfEditor.Controls;
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
        private TabControl _tabPreviewer;

        private Dictionary<string, TabPage> _previewerDict = new Dictionary<string, TabPage>();
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
                    var dirNode = new TreeNode(Path.GetFileName(item.Key)) { Tag = item.Key };
                    _tvPdf.Nodes.Add(dirNode);
                    foreach (var pdf in item.Value)
                    {
                        var pdfNode = new TreeNode(Path.GetFileName(pdf)) { Tag = pdf };
                        dirNode.Nodes.Add(pdfNode);
                    }
                }
            };
            background.RunWorkerAsync();
        }
        #endregion

        #region event handler
        private void TvPdf_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string path && Path.GetExtension(path).ToLower() == ".pdf")
            {
                if (_previewerDict.ContainsKey(path))
                {
                    _tabPreviewer.SelectedTab = _previewerDict[path];
                }
                else
                {
                    var tabPage = new TabPage(Path.GetFileName(path));
                    var previewer = new BrowserPreviewer { Dock = DockStyle.Fill };
                    previewer.OpenPdf(path);
                    tabPage.Controls.Add(previewer);
                    _tabPreviewer.TabPages.Add(tabPage);
                    _tabPreviewer.SelectedTab = tabPage;
                    _previewerDict.Add(path, tabPage);
                }
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            _tvPdf = new TreeView
            {
                Dock = DockStyle.Left,
                Width = 250
            };
            _tvPdf.NodeMouseDoubleClick += TvPdf_NodeMouseDoubleClick;
            Controls.Add(_tvPdf);

            _tabPreviewer = new TabControl
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(_tabPreviewer);
            _tabPreviewer.BringToFront();
        }
        #endregion
    }
}
