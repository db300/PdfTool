using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF预览面板
    /// </summary>
    public partial class PdfPreviewPanel : UserControl
    {
        #region constructor
        public PdfPreviewPanel()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private PdfHelperLibrary.ImagerHelper2 _helper;
        private TreeView _tvPages;
        private PictureBox _picPage;
        #endregion

        #region method
        public void OpenPdf(string fileName)
        {
            _tvPages.Nodes.Clear();
            _picPage.Image = null;
            var pageCount = PdfHelperLibrary.CommonHelper.GetPageCount(fileName);
            for (var i = 1; i <= pageCount; i++)
            {
                _tvPages.Nodes.Add(i.ToString());
            }
            _helper = new PdfHelperLibrary.ImagerHelper2(fileName);
        }
        #endregion

        #region event handler
        private void TvPages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node is null)
            {
                _picPage.Image = null;
                return;
            }
            var pageNum = int.Parse(e.Node.Text) - 1;
            _picPage.Image = _helper.GetPageImage(pageNum, 100);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            _tvPages = new TreeView
            {
                Dock = DockStyle.Left,
                FullRowSelect = true,
                HideSelection = false,
                Parent = this,
                ShowLines = false,
                Width = 150
            };
            _tvPages.AfterSelect += TvPages_AfterSelect;
            _picPage = new PictureBox
            {
                Dock = DockStyle.Fill,
                Parent = this,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            _picPage.BringToFront();
        }
        #endregion
    }
}
