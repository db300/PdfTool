using PdfSharp.Pdf.IO;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfMerger
{
    public partial class MainForm : Form
    {
        #region constructor
        public MainForm()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TextBox _txtFileList;
        private TextBox _txtLog;

        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var inputFileList = openDlg.FileNames.ToList();
            foreach (var fileName in inputFileList)
            {
                var document = PdfReader.Open(fileName, PdfDocumentOpenMode.Import);
                _txtLog.AppendText($"【页数：{document.PageCount}】{fileName}\r\n");
                _txtFileList.AppendText($"{fileName}\r\n");
            }
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
            PdfHelperLibrary.MergeHelper.MergePdf(inputPdfFilenameList);
            _txtLog.AppendText("合并完成\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF合并器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var btnMerge = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始合并"
            };
            btnMerge.Click += BtnMerge_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(btnAddFile.Left, ClientSize.Height - ControlMargin - 200),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - 2 * btnAddFile.Left, 200),
                WordWrap = false
            };

            _txtFileList = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + ControlPadding),
                Multiline = true,
                Parent = this,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - 2 * btnAddFile.Left, _txtLog.Top - btnAddFile.Bottom - 2 * ControlPadding),
                WordWrap = false
            };
        }
        #endregion
    }
}
