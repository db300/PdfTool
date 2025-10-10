using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExcelTool.Modules
{
    /// <summary>
    /// 数据提取器
    /// </summary>
    public partial class DataExtracter : UserControl
    {
        #region constructor
        public DataExtracter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private CheckedListBox _chkFields;
        private TextBox _txtLog;
        #endregion

        #region method
        public void OpenFile(List<string> files)
        {
            _txtLog.Clear();
            _inputFileList.Clear();
            _inputFileList.AddRange(files);
            _chkFields.Items.Clear();
            foreach (var fileName in _inputFileList)
            {
                _txtLog.AppendText($"【{DateTime.Now:yyyyMMddHHmmss}】{fileName}\r\n");
                var headers = Helpers.DataExtractHelper.ExtractHeader(fileName, msg =>
                {
                    _txtLog.AppendText($"{msg}\r\n");
                });
                if (headers?.Count > 0)
                {
                    _chkFields.Items.AddRange(headers.ToArray());
                }
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|csv文件(*.csv)|*.csv|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenFile(openDlg.FileNames.ToList());
        }

        private void BtnExtract_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要提取的Excel文件\r\n";
                return;
            }
            var fields = _chkFields.CheckedIndices.Cast<string>().ToList();
            if (fields.Count == 0)
            {
                _txtLog.Text = "未选择提取字段\r\n";
                return;
            }
            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                foreach (var file in _inputFileList)
                {
                    Helpers.DataExtractHelper.Extract(file, fields, msg =>
                    {
                        background.ReportProgress(0, msg);
                    });
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
                _txtLog.AppendText($"提取完成\r\n");
            };
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
                Location = new Point(btnAddFile.Right + Config.ControlPadding, Config.ControlMargin),
                Parent = this,
                Text = "开始提取"
            };
            btnExtract.Click += BtnExtract_Click;

            _chkFields = new CheckedListBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Parent = this,
                Size = new Size(200, ClientSize.Height - Config.ControlMargin - btnAddFile.Bottom - Config.ControlPadding)
            };

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F),
                Location = new Point(_chkFields.Right + Config.ControlPadding, btnAddFile.Bottom + Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin - _chkFields.Right - Config.ControlPadding, ClientSize.Height - Config.ControlMargin - btnAddFile.Bottom - Config.ControlPadding),
                WordWrap = false
            };
        }
        #endregion
    }
}
