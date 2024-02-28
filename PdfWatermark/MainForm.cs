using PdfWatermark.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfWatermark
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
        private TextBox _txtWatermark;
        private TextBox _txtFileList;
        private TextBox _txtLog;

        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var inputFileList = openDlg.FileNames.ToList();
            foreach (var fileName in inputFileList)
            {
                _txtLog.AppendText($"【页数：{PdfHelperLibrary.CommonHelper.GetPageCount(fileName)}】{fileName}\r\n");
                _txtFileList.AppendText($"{fileName}\r\n");
            }
        }

        private void BtnAddWatermark_Click(object sender, EventArgs e)
        {
            var inputPdfFilenameList = _txtFileList.Lines.ToList();
            inputPdfFilenameList.RemoveAll(string.IsNullOrWhiteSpace);
            if (inputPdfFilenameList.Count == 0)
            {
                _txtLog.AppendText("未添加需要添加水印的PDF文件\r\n");
                return;
            }
            foreach (var file in inputPdfFilenameList)
            {
                var s = PdfHelperLibrary.WatermarkHelper.WatermarkPdf(file, _txtWatermark.Text);
                if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"{file} 水印添加完成\r\n");
                else _txtLog.AppendText($"{s}\r\n");
            }
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
            Text = $"PDF水印工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            _txtWatermark = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(btnAddFile.Left, btnAddFile.Bottom + ControlPadding),
                Parent = this,
                Text = "这是一个水印",
                Width = 250
            };
            var btnAddWatermark = new Button
            {
                AutoSize = true,
                Location = new Point(_txtWatermark.Right + ControlPadding, _txtWatermark.Top - 1),
                Parent = this,
                Text = "添加水印"
            };
            btnAddWatermark.Click += BtnAddWatermark_Click;

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
                Location = new Point(btnAddFile.Left, _txtWatermark.Bottom + ControlPadding),
                Multiline = true,
                Parent = this,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - 2 * btnAddFile.Left, _txtLog.Top - _txtWatermark.Bottom - 2 * ControlPadding),
                WordWrap = false
            };

            var picAppreciate = new PictureBox
            {
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
