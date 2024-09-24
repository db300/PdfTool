using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF合并器
    /// </summary>
    public partial class PdfMerger : UserControl
    {
        #region constructor
        public PdfMerger()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TextBox _txtFileList;
        private CheckBox _ckbAutoOpen;
        private CheckBox _ckbAddBookmarks;
        private TextBox _txtLog;
        #endregion

        #region method
        public void OpenPdfs(List<string> files)
        {
            foreach (var fileName in files)
            {
                _txtLog.AppendText($"【页数：{PdfHelperLibrary.CommonHelper.GetPageCount(fileName)}】{fileName}\r\n");
                _txtFileList.AppendText($"{fileName}\r\n");
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

        private void BtnMerge_Click(object sender, EventArgs e)
        {
            var inputPdfFilenameList = _txtFileList.Lines.ToList();
            inputPdfFilenameList.RemoveAll(string.IsNullOrWhiteSpace);
            if (inputPdfFilenameList.Count == 0)
            {
                _txtLog.AppendText("未添加需要合并的PDF文件\r\n");
                return;
            }
            var s = PdfHelperLibrary.MergeHelper.MergePdf(inputPdfFilenameList, _ckbAutoOpen.Checked, _ckbAddBookmarks.Checked, out var outputPdfFilename);
            if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"合并完成: {outputPdfFilename}\r\n");
            else _txtLog.AppendText($"{s}\r\n");
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

            var btnMerge = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始合并"
            };
            btnMerge.Click += BtnMerge_Click;

            _ckbAutoOpen = new CheckBox
            {
                AutoSize = true,
                Parent = this,
                Text = "合并后自动打开"
            };
            _ckbAutoOpen.Location = new Point(btnMerge.Right + Config.ControlPadding, btnMerge.Top + (btnMerge.Height - _ckbAutoOpen.Height) / 2);

            _ckbAddBookmarks = new CheckBox
            {
                AutoSize = true,
                Location = new Point(_ckbAutoOpen.Right + Config.ControlPadding, _ckbAutoOpen.Top),
                Parent = this,
                Text = "将每个文件名添加至书签"
            };

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, ClientSize.Height - Config.ControlMargin - 200),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 200),
                WordWrap = false
            };

            _txtFileList = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, _txtLog.Top - btnAddFile.Bottom - 2 * Config.ControlPadding),
                WordWrap = false
            };
        }
        #endregion
    }
}
