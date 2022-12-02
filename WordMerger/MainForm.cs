using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WordMerger
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
        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "Word文件(*.docx)|*.docx|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var inputFileList = openDlg.FileNames.ToList();
            foreach (var fileName in inputFileList)
            {
                _txtFileList.AppendText($"{fileName}\r\n");
            }
        }

        private void BtnMerge_Click(object sender, EventArgs e)
        {
            var inputFilenameList = _txtFileList.Lines.ToList();
            inputFilenameList.RemoveAll(string.IsNullOrWhiteSpace);
            if (inputFilenameList.Count == 0)
            {
                _txtLog.AppendText("未添加需要合并的Word文件\r\n");
                return;
            }
            var path = Path.GetDirectoryName(inputFilenameList.First());
            var ext = Path.GetExtension(inputFilenameList.First());
            var outputFileName = Path.Combine(path, $"MergedFile - {DateTime.Now:yyyyMMddHHmmssfff}{ext}");
            var s = WordHelperLibrary.MergeHelper.Merge(inputFilenameList, outputFileName);
            if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText("合并完成\r\n");
            else _txtLog.AppendText($"{s}\r\n");
        }

        private void BtnAppreciate_Click(object sender, EventArgs e)
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
            Text = $"Word文档合并器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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

            var btnAppreciate = new Button
            {
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                Parent = this,
                Text = "快来打赏我吧"
            };
            btnAppreciate.Location = new Point(ClientSize.Width - btnAppreciate.Width - 6, 6);
            btnAppreciate.Click += BtnAppreciate_Click;
        }
        #endregion
    }
}
