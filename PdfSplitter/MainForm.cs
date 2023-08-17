using PdfSplitter.Properties;
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
        private NumericUpDown _numPageFrom;
        private NumericUpDown _numPageTo;

        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
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
                _txtLog.Text = "未添加需要拆分的PDF文件\r\n";
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
                _txtLog.Text = "未添加需要提取的PDF文件\r\n";
                return;
            }
            if (_inputPdfFileList.Count != 1)
            {
                _txtLog.Text = "添加了多个PDF文件，只对第一个文件进行提取\r\n";
            }
            var s = PdfHelperLibrary.ExtractHelper.ExtractPdf(_inputPdfFileList[0], (int)_numPageFrom.Value, (int)_numPageTo.Value, out var outputPdfFile);
            if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"{_inputPdfFileList[0]} 提取完成: {outputPdfFile}\r\n");
            else _txtLog.AppendText($"{s}\r\n");
        }

        private void PicAppreciate_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Appreciate);
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

            var page1 = new TabPage { BorderStyle = BorderStyle.None, Name = "tpDefault", Text = "常规拆分" };
            var page2 = new TabPage { BorderStyle = BorderStyle.None, Name = "tpSpecialPage", Text = "指定页提取" };
            var tab4SplitMode = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + ControlPadding),
                Parent = this,
                Size = new Size(ClientSize.Width - ControlMargin * 2, 150)
            };
            tab4SplitMode.TabPages.Add(page1);
            tab4SplitMode.TabPages.Add(page2);

            //常规拆分
            _numPagePerDoc = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Maximum = 100000,
                Minimum = 1,
                Parent = page1,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 50
            };
            var lbl = new Label { AutoSize = true, Parent = page1, Text = "页/文档" };
            lbl.Location = new Point(_numPagePerDoc.Right + 6, _numPagePerDoc.Top + (_numPagePerDoc.Height - lbl.Height) / 2);
            var btnSplit = new Button { Anchor = AnchorStyles.Left | AnchorStyles.Bottom, AutoSize = true, Parent = page1, Text = "开始拆分" };
            btnSplit.Location = new Point(ControlMargin, page1.ClientSize.Height - ControlMargin - btnSplit.Height);
            btnSplit.Click += BtnSplit_Click;

            //指定页提取
            lbl = new Label { AutoSize = true, Location = new Point(ControlMargin, ControlMargin), Parent = page2, Text = "从：" };
            _numPageFrom = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Maximum = 100000,
                Minimum = 1,
                Parent = page2,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };
            lbl = new Label { AutoSize = true, Location = new Point(_numPageFrom.Right + ControlPadding, lbl.Top), Parent = page2, Text = "到：" };
            _numPageTo = new NumericUpDown
            {
                AutoSize = true,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Maximum = 100000,
                Minimum = 1,
                Parent = page2,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };
            var btnExtract = new Button { Anchor = AnchorStyles.Left | AnchorStyles.Bottom, AutoSize = true, Parent = page2, Text = "开始提取" };
            btnExtract.Location = new Point(ControlMargin, page2.ClientSize.Height - ControlMargin - btnExtract.Height);
            btnExtract.Click += BtnExtract_Click;

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

            var picAppreciate = new PictureBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand,
                Image = Resources.appreciatesmall,
                Location = new Point(ClientSize.Width - 32 - 6, 6),
                Parent = this,
                Size = new Size(32, 32),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            picAppreciate.Click += PicAppreciate_Click;
        }
        #endregion
    }
}
