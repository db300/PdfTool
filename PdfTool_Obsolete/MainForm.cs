using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PdfTool
{
    public partial class MainForm : Form
    {
        #region constructor
        public MainForm()
        {
            InitializeComponent();

            /*
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            */

            InitUi();
        }
        #endregion
        #region property
        private const int SrcFileNameColIndex = 0;
        private const int ProgressColIndex = 1;
        private List<PdfFileItem> _pdfFileList;
        #endregion
        #region event handler

        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            _pdfFileList = openDlg.FileNames.ToList().Select(a => new PdfFileItem
            {
                SrcFileName = a,
                Progress = "待处理"
            }).ToList();
            var data = _pdfFileList.Select(a => new ListViewItem(new[] { a.SrcFileName, a.Progress })).ToArray();
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var pdfFileItem in _pdfFileList)
                {
                    pdfFileItem.TextLines = TextExtractor.Pdf2String(pdfFileItem.SrcFileName);
                    pdfFileItem.Progress = "文本提取完成";
                    ((BackgroundWorker)ww).ReportProgress(_pdfFileList.IndexOf(pdfFileItem), pdfFileItem);

                    var outFileName = $"{Config.OutputDir}{Path.GetFileNameWithoutExtension(pdfFileItem.SrcFileName)}.docx";
                    OfficeHelperLibrary.WordHelper.CreateFile(outFileName, pdfFileItem.TextLines);
                    pdfFileItem.DesFileName = outFileName;
                    pdfFileItem.Progress = "Word生成完成";
                    ((BackgroundWorker)ww).ReportProgress(_pdfFileList.IndexOf(pdfFileItem), pdfFileItem);
                }
            };
            background.ProgressChanged += (ww, ee) =>
            {
            };
            background.RunWorkerAsync();
        }
        #endregion
        #region ui
        private void InitUi()
        {
            Size = new Size(1600, 900);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PDF Tool";

            var btnAddFile = new Button
            {
                Location = new Point(10, 80),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            var btnConvert = new Button
            {
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                Parent = this,
                Text = "开始转换"
            };
            btnConvert.Location = new Point(ClientSize.Width - 10 - btnConvert.Width, btnAddFile.Bottom + 10);
            btnConvert.Click += BtnConvert_Click;
        }
        #endregion
    }
}
