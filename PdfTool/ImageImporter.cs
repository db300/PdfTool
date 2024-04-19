using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// 图片导入器
    /// </summary>
    public partial class ImageImporter : UserControl
    {
        #region constructor
        public ImageImporter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TextBox _txtFileList;
        private TextBox _txtLog;
        #endregion

        #region method
        public void OpenImages(List<string> files)
        {
            foreach (var fileName in files)
            {
                _txtFileList.AppendText($"{fileName}\r\n");
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "图片文件(*.bmp;*.jpg;*.tif;*.png)|*.bmp;*.jpg;*.tif;*.png|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenImages(openDlg.FileNames.ToList());
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var inputImgFileNameList = _txtFileList.Lines.ToList();
            inputImgFileNameList.RemoveAll(string.IsNullOrWhiteSpace);
            if (inputImgFileNameList.Count == 0)
            {
                _txtLog.AppendText("未添加需要导入的图片文件\r\n");
                return;
            }
            var s = PdfHelperLibrary.ImageHelper.ConvertImageToPdf(inputImgFileNameList, out var outputPdfFileName);
            if (string.IsNullOrWhiteSpace(s)) _txtLog.AppendText($"导入完成: {outputPdfFileName}\r\n");
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

            var btnImport = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始导入"
            };
            btnImport.Click += BtnImport_Click;

            var ckbOneImgOnePage = new CheckBox
            {
                AutoSize = true,
                Checked = true,
                Enabled = false,
                Location = new Point(btnImport.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "每张图片占一页"
            }; 
            ckbOneImgOnePage.Top = btnImport.Top + (btnImport.Height - ckbOneImgOnePage.Height) / 2;

            var ckbKeepImageSize = new CheckBox
            {
                AutoSize = true,
                Checked = true,
                Enabled = false,
                Location = new Point(ckbOneImgOnePage.Right + Config.ControlPadding, ckbOneImgOnePage.Top),
                Parent = this,
                Text = "保持原始图片大小"
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
