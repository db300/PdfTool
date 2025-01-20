using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF表格提取器
    /// </summary>
    public partial class PdfTableExtracter : UserControl, IPdfHandler
    {
        #region constructor
        public PdfTableExtracter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        private TextBox _txtLog;
        #endregion

        #region method
        public void OpenPdfs(List<string> files)
        {
            _txtLog.Clear();
            _inputPdfFileList.Clear();
            _inputPdfFileList.AddRange(files);
            foreach (var fileName in _inputPdfFileList)
            {
                _txtLog.AppendText($"【页数：{PdfHelperLibrary.CommonHelper.GetPageCount(fileName)}】{fileName}\r\n");
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenPdfs(openDlg.FileNames.ToList());
        }

        private void BtnExtract_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要提取的PDF文件\r\n";
                return;
            }
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var fileName in _inputPdfFileList)
                {
                    var (success, s, pdfExtractTables) = PdfHelperLibrary.TableExtractHelper.ExtractTable(fileName);
                    var msg = success ? $"{fileName} 提取完成" : $"{fileName} {s}";
                    background.ReportProgress(0, msg);
                    var sb = new StringBuilder();
                    foreach (var table in pdfExtractTables)
                    {
                        foreach (var row in table.Rows)
                        {
                            sb.AppendLine(string.Join("\t", row.Cells.Select(a => a.Replace("\r", ""))));
                        }
                    }
                    background.ReportProgress(0, sb.ToString());

                    var outputFileName = fileName.Replace(".pdf", ".xlsx");
                    var commonTable = new ExcelHelperLibrary.CommonTable
                    {
                        Rows = pdfExtractTables.SelectMany(table => table.Rows)
                           .Select(row => new ExcelHelperLibrary.CommonRow { Cells = row.Cells })
                           .ToList()
                    };
                    ExcelHelperLibrary.GenerateHelper.GenerateExcel(outputFileName, new List<ExcelHelperLibrary.CommonTable> { commonTable });
                    background.ReportProgress(0, $"{outputFileName} 生成完成");
                }
            };
            background.ProgressChanged += (ww, ee) => { if (ee.UserState is string msg) { _txtLog.AppendText($"{msg}\r\n"); } };
            background.RunWorkerCompleted += (ww, ee) => { _txtLog.AppendText($"提取完成\r\n"); };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在提取，请稍候...\r\n");
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

            var btnExtract = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, btnAddFile.Top),
                Parent = this,
                Text = "开始提取"
            };
            btnExtract.Click += BtnExtract_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - btnAddFile.Bottom - Config.ControlPadding),
                WordWrap = false
            };
        }
        #endregion
    }
}
