using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private NumericUpDown _numPagePerDoc;

        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
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
                PdfHelperLibrary.SplitHelper.SplitPdf(fileName, (int)_numPagePerDoc.Value);
                _txtLog.AppendText($"{fileName} 拆分完成\r\n");
            }
            _txtLog.AppendText("拆分完成\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF拆分器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var btnSplit = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始拆分"
            };
            btnSplit.Click += BtnSplit_Click;

            var gpOptions = new GroupBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - 2 * btnAddFile.Left, 50),
                Text = "拆分选项"
            };

            _numPagePerDoc = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Maximum = 1000,
                Minimum = 1,
                Parent = gpOptions,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 50
            };
            var lbl = new Label
            {
                AutoSize = true,
                Parent = gpOptions,
                Text = "页/文档"
            };
            lbl.Location = new Point(_numPagePerDoc.Right + 6, _numPagePerDoc.Top + (_numPagePerDoc.Height - lbl.Height) / 2);

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(btnAddFile.Left, gpOptions.Bottom + ControlPadding),
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
