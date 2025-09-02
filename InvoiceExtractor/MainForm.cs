using CsvHelper;
using CsvHelper.Configuration;
using InvoiceHelperLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InvoiceExtractor
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

        private static readonly List<string> Columns = new List<string>
        {
            "发票类型",
            "发票号码",
            "开票日期",
            "购买方名称",
            "购买方税号",
            "销售方名称",
            "销售方税号",
            "金额合计",
            "税额合计"
        };

        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
        #endregion

        #region method
        private void ExportCsv(List<InvoiceItem> list, out string outputFileName)
        {
            outputFileName = $"发票信息提取结果_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };

            using (var writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, config))
            {
                // 写表头
                foreach (var column in Columns)
                {
                    csv.WriteField(column);
                }
                csv.NextRecord();

                // 写数据
                foreach (var item in list)
                {
                    csv.WriteField(item.InvoiceType);
                    csv.WriteField(item.InvoiceNumber);
                    csv.WriteField(item.InvoiceDate);
                    csv.WriteField(item.BuyerName);
                    csv.WriteField(item.BuyerTaxNumber);
                    csv.WriteField(item.SellerName);
                    csv.WriteField(item.SellerTaxNumber);
                    csv.WriteField(item.TotalAmount);
                    csv.WriteField(item.TotalTax);
                    csv.NextRecord();
                }
            }
        }
        #endregion

        #region event handler
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;

            _txtLog.Clear();
            _inputPdfFileList.Clear();
            _inputPdfFileList.AddRange(openDlg.FileNames.ToList());
            foreach (var fileName in _inputPdfFileList)
            {
                _txtLog.AppendText($"{fileName}\r\n");
            }
        }

        private void BtnExtract_Click(object sender, EventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要提取的发票文件\r\n";
                return;
            }
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                var list = new List<InvoiceItem>();
                foreach (var fileName in _inputPdfFileList)
                {
                    var (success, msg, invoiceItem) = InvoiceHelperLibrary.ParseHelper.Extract(fileName);
                    if (!success)
                    {
                        background.ReportProgress(0, $"{fileName} 提取失败: {msg}");
                        continue;
                    }
                    list.Add(invoiceItem);
                    background.ReportProgress(0, $"{fileName} 提取成功");
                }

                ee.Result = list;
            };
            background.ProgressChanged += (ww, ee) => { if (ee.UserState is string msg) { _txtLog.AppendText($"{msg}\r\n"); } };
            background.RunWorkerCompleted += (ww, ee) =>
            {
                _txtLog.AppendText($"提取完成\r\n");
                if (ee.Result is List<InvoiceItem> list)
                {
                    ExportCsv(list, out var outputFileName);
                    _txtLog.AppendText($"{outputFileName} 生成完成\r\n");

                    var sb = new StringBuilder();
                    sb.AppendLine(string.Join(",", Columns));
                    foreach (var item in list)
                    {
                        sb.AppendLine($"{item.InvoiceType},{item.InvoiceNumber},{item.InvoiceDate},{item.BuyerName},{item.BuyerTaxNumber},{item.SellerName},{item.SellerTaxNumber},{item.TotalAmount},{item.TotalTax}");
                    }
                    /*
                    var csvFileName = outputFileName.Replace(".csv", "_summary.csv");
                    File.WriteAllText(csvFileName, sb.ToString(), Encoding.UTF8);
                    */
                    _txtLog.AppendText($"{sb}\r\n");
                }
            };
            background.RunWorkerAsync();
            _txtLog.AppendText($"正在提取，请稍候...\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            ClientSize = new Size(1000, 800);
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"发票信息提取工具 v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var btnOpen = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "选择文件"
            };
            btnOpen.Click += BtnOpen_Click;

            var btnExtract = new Button
            {
                AutoSize = true,
                Location = new Point(btnOpen.Right + ControlPadding, ControlMargin),
                Parent = this,
                Text = "提取数据"
            };
            btnExtract.Click += BtnExtract_Click;

            var top = btnOpen.Bottom + ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F),
                Location = new Point(ControlMargin, top),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - 2 * ControlMargin, ClientSize.Height - ControlMargin - top),
                WordWrap = false
            };
        }
        #endregion
    }
}
