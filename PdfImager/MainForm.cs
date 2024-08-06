using PdfImager.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfImager
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
        private ComboBox _cmbDpi;

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

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要转换的PDF文件\r\n";
                return;
            }
            var dpi = int.Parse(_cmbDpi.Text);
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var fileName in _inputPdfFileList)
                {
                    var s = PdfHelperLibrary.ImagerHelper.ConvertPdfToImage(fileName, dpi, "png", info => background.ReportProgress(0, info));
                    var msg = string.IsNullOrWhiteSpace(s) ? $"{fileName} 转换完成" : $"{fileName} {s}";
                    background.ReportProgress(0, msg);
                }
            };
            background.ProgressChanged += (ww, ee) =>
            {
                if (ee.UserState is string msg)
                {
                    _txtLog.AppendText($"{msg}\r\n");
                }
            };
            background.RunWorkerCompleted += (ww, ee) =>
            {
                _txtLog.AppendText($"转换完成\r\n");
            };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在转换，请稍候...\r\n");
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
            Text = $"PDF转图器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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

            var lbl = new Label
            {
                AutoSize = true,
                Location = new Point(ControlMargin, btnAddFile.Bottom + ControlPadding),
                Parent = this,
                Text = "生成图片DPI："
            };
            _cmbDpi = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lbl.Right, lbl.Top - 3),
                Parent = this
            };
            _cmbDpi.Items.AddRange(new object[] { 100, 200, 300, 600, 900, 1200 });
            _cmbDpi.SelectedIndex = 2;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(ControlMargin, _cmbDpi.Bottom + ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - ControlMargin * 2, ClientSize.Height - ControlMargin - _cmbDpi.Bottom - ControlPadding),
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
