using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordToPdf.Properties;

namespace WordToPdf
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
        private readonly List<string> _inputWordFileList = new List<string>();
        private TextBox _txtLog;

        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "Word文件(*.docx;*.doc)|*.docx;*.doc|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            _txtLog.Clear();
            _inputWordFileList.Clear();
            _inputWordFileList.AddRange(openDlg.FileNames.ToList());
            foreach (var fileName in _inputWordFileList)
            {
                _txtLog.AppendText($"{fileName}\r\n");
            }
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            if (_inputWordFileList.Count == 0)
            {
                _txtLog.AppendText("未添加需要转换的Word文件\r\n");
                return;
            }
            _txtLog.AppendText($"正在转换，请稍候...\r\n");
            Helper.ConvertToPdf(_inputWordFileList, msg =>
            {
                _txtLog.AppendText($"{msg}\r\n");
            });
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
            Text = $"Word转PDF(批量) {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var btnConvert = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始转换"
            };
            btnConvert.Click += BtnConvert_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(ControlMargin, btnAddFile.Bottom + ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - ControlMargin * 2, ClientSize.Height - ControlMargin - btnAddFile.Bottom - ControlPadding),
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
