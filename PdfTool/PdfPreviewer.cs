using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF预览器
    /// </summary>
    public partial class PdfPreviewer : UserControl
    {
        #region constructor
        public PdfPreviewer()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TabControl _tabControl;
        #endregion

        #region method
        public void OpenPdfs(List<string> files)
        {
            var exceptList = files.Except(_inputPdfFileList).ToList();
            _inputPdfFileList.AddRange(exceptList);
            foreach (var fileName in exceptList)
            {
                var viewPanel = new PdfPreviewPanel { Dock = DockStyle.Fill };
                viewPanel.OpenPdf(fileName);
                var tabPage = new TabPage { BorderStyle = BorderStyle.None, Tag = fileName, Text = Path.GetFileName(fileName) };
                tabPage.Controls.Add(viewPanel);
                _tabControl.TabPages.Add(tabPage);
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

            _tabControl = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - btnAddFile.Bottom - Config.ControlPadding - Config.ControlMargin)
            };
        }
        #endregion
    }
}
