using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF拆分器
    /// </summary>
    public partial class PdfSplitter : UserControl
    {
        #region constructor
        public PdfSplitter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtLog;
        private NumericUpDown _numPagePerDoc;
        private NumericUpDown _numPageFrom;
        private NumericUpDown _numPageTo;
        private TextBox _txtDeletePageNum;

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
                _txtLog.AppendText($"【页数：{PdfHelperLibrary.CommonHelper.GetPageCount(fileName)}】{fileName}\r\n");
            }
        }

        private void BtnSplit_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.AppendText("未添加需要拆分的PDF文件\r\n");
                return;
            }
            foreach (var fileName in _inputPdfFileList)
            {
                var s = PdfHelperLibrary.SplitHelper.SplitPdf(fileName, (int)_numPagePerDoc.Value);
                if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"{fileName} 拆分完成\r\n");
                else _txtLog.AppendText($"{fileName} {s}\r\n");
            }
            _txtLog.AppendText("拆分完成\r\n");
        }

        private void BtnExtract_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.AppendText("未添加需要提取的PDF文件\r\n");
                return;
            }
            foreach (var fileName in _inputPdfFileList)
            {
                var s = PdfHelperLibrary.ExtractHelper.ExtractPdf(fileName, (int)_numPageFrom.Value, (int)_numPageTo.Value, out var outputPdfFile);
                if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"{fileName} 提取完成: {outputPdfFile}\r\n");
                else _txtLog.AppendText($"{s}\r\n");
            }
            _txtLog.AppendText("提取完成\r\n");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.AppendText("未添加需要删页的PDF文件\r\n");
                return;
            }
            var pageNums = _txtDeletePageNum.Text.Split(new[] { ',', ';', '，', '；' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (!(pageNums?.Count > 0))
            {
                _txtLog.AppendText("未输入要删除的页码\r\n");
                return;
            }
            foreach (var fileName in _inputPdfFileList)
            {
                var s = PdfHelperLibrary.ExtractHelper.DeletePdfPage(fileName, pageNums, out var outputPdfFile);
                if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"{fileName} 删页完成: {outputPdfFile}\r\n");
                else _txtLog.AppendText($"{s}\r\n");
            }
            _txtLog.AppendText("删页完成\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var page1 = new TabPage { BorderStyle = BorderStyle.None, Name = "tpDefault", Text = "常规拆分" };
            var page2 = new TabPage { BorderStyle = BorderStyle.None, Name = "tpSpecialPageExtract", Text = "指定页提取" };
            var page3 = new TabPage { BorderStyle = BorderStyle.None, Name = "tpSpecialPageDelete", Text = "指定页删除" };
            var tab4SplitMode = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - ControlMargin * 2, 150)
            };
            tab4SplitMode.TabPages.Add(page1);
            tab4SplitMode.TabPages.Add(page2);
            tab4SplitMode.TabPages.Add(page3);

            //常规拆分
            InitUi4Common(page1);
            //指定页提取
            InitUi4Extract(page2);
            //指定页删除
            InitUi4Delete(page3);

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(ControlMargin, tab4SplitMode.Bottom + ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - ControlMargin * 2, ClientSize.Height - ControlMargin - tab4SplitMode.Bottom - ControlPadding),
                WordWrap = false
            };
        }

        private void InitUi4Common(TabPage tabPage)
        {
            _numPagePerDoc = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Maximum = 100000,
                Minimum = 1,
                Parent = tabPage,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 50
            };
            var lbl = new Label { AutoSize = true, Parent = tabPage, Text = "页/文档" };
            lbl.Location = new Point(_numPagePerDoc.Right + 6, _numPagePerDoc.Top + (_numPagePerDoc.Height - lbl.Height) / 2);
            var btnSplit = new Button { Anchor = AnchorStyles.Left | AnchorStyles.Bottom, AutoSize = true, Parent = tabPage, Text = "开始拆分" };
            btnSplit.Location = new Point(ControlMargin, tabPage.ClientSize.Height - ControlMargin - btnSplit.Height);
            btnSplit.Click += BtnSplit_Click;
        }

        private void InitUi4Extract(TabPage tabPage)
        {
            var lbl = new Label { AutoSize = true, Location = new Point(ControlMargin, ControlMargin), Parent = tabPage, Text = "从：" };
            _numPageFrom = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Maximum = 100000,
                Minimum = 1,
                Parent = tabPage,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };
            lbl = new Label { AutoSize = true, Location = new Point(_numPageFrom.Right + ControlPadding, lbl.Top), Parent = tabPage, Text = "到：" };
            _numPageTo = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Maximum = 100000,
                Minimum = 1,
                Parent = tabPage,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };
            var btnExtract = new Button { Anchor = AnchorStyles.Left | AnchorStyles.Bottom, AutoSize = true, Parent = tabPage, Text = "开始提取" };
            btnExtract.Location = new Point(ControlMargin, tabPage.ClientSize.Height - ControlMargin - btnExtract.Height);
            btnExtract.Click += BtnExtract_Click;
        }

        private void InitUi4Delete(TabPage tabPage)
        {
            var lbl = new Label { AutoSize = true, Location = new Point(ControlMargin, ControlMargin), Parent = tabPage, Text = "删除页码：" };
            _txtDeletePageNum = new TextBox
            {
                Location = new Point(lbl.Right, lbl.Top - 4),
                Parent = tabPage,
                Width = 200
            };
            lbl = new Label
            {
                AutoSize = true,
                ForeColor = Color.Blue,
                Location = new Point(_txtDeletePageNum.Right, _txtDeletePageNum.Top + 4),
                Parent = tabPage,
                Text = "输入要删除的页码，多个页码可用逗号或分号间隔"
            };

            var btnDelete = new Button { Anchor = AnchorStyles.Left | AnchorStyles.Bottom, AutoSize = true, Parent = tabPage, Text = "开始删除" };
            btnDelete.Location = new Point(ControlMargin, tabPage.ClientSize.Height - ControlMargin - btnDelete.Height);
            btnDelete.Click += BtnDelete_Click;
        }
        #endregion
    }
}
