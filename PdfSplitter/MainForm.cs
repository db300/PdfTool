using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
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

namespace PdfSplitter
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
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            _txtLog.Clear();
            _inputPdfFileList.Clear();
            _inputPdfFileList.AddRange(openDlg.FileNames.ToList());
            foreach (var fileName in _inputPdfFileList)
            {
                var document = PdfReader.Open(fileName, PdfDocumentOpenMode.Import);
                _txtLog.AppendText($"【页数：{document.PageCount}】{fileName}\r\n");
            }
        }

        private void BtnSplit_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要拆分的PDF文件";
                return;
            }
            foreach (var fileName in _inputPdfFileList)
            {
                SplitPdf(fileName);
                _txtLog.AppendText($"{fileName} 拆分完成\r\n");
            }
            _txtLog.AppendText("拆分完成\r\n");
        }
        #endregion

        #region method
        private void SplitPdf(string inputPdfFileName)
        {
            var inputDocument = PdfReader.Open(inputPdfFileName, PdfDocumentOpenMode.Import);
            var pageCount = inputDocument.PageCount;
            var path = Path.GetDirectoryName(inputPdfFileName);
            var fileName = Path.GetFileNameWithoutExtension(inputPdfFileName);
            var prefixFileName = Path.Combine(path, fileName);
            for (var i = 0; i < pageCount; i++)
            {
                // Create new document
                var outputDocument = new PdfDocument { Version = inputDocument.Version };
                outputDocument.Info.Title = $"Page {i + 1} of {inputDocument.Info.Title}";
                outputDocument.Info.Creator = inputDocument.Info.Creator;
                // Add the page and save it
                outputDocument.AddPage(inputDocument.Pages[i]);
                outputDocument.Save($"{prefixFileName} - Page {i + 1}.pdf");
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PDF拆分器";

            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(20, 20),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var btnSplit = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + 12, btnAddFile.Top),
                Parent = this,
                Text = "开始拆分"
            };
            btnSplit.Click += BtnSplit_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + 12),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                WordWrap = false
            };
            _txtLog.Size = new Size(ClientSize.Width - _txtLog.Left * 2, ClientSize.Height - _txtLog.Left - _txtLog.Top);
        }
        #endregion
    }
}
