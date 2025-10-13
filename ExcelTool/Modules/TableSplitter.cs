using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelTool.Modules
{
    /// <summary>
    /// 表格拆分器
    /// </summary>
    public partial class TableSplitter : UserControl, IExcelHandler
    {
        #region constructor
        public TableSplitter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private TextBox _txtLog;
        private NumericUpDown _numHeaderRow;
        private NumericUpDown _numMaxRows;
        #endregion

        #region method
        public void OpenExcels(List<string> files)
        {
            _txtLog.Clear();
            _inputFileList.Clear();
            _inputFileList.AddRange(files);
            foreach (var fileName in _inputFileList)
            {
                _txtLog.AppendText($"【{DateTime.Now:yyyyMMddHHmmss}】{fileName}\r\n");
            }
        }

        private void ProcessFile(string fileName, int headerRows, int maxRows)
        {
            try
            {
                var ext = Path.GetExtension(fileName).ToLower();
                switch (ext)
                {
                    case ".xls":
                    case ".xlsx":
                        {
                            Log("------------------------------------");
                            Log($"开始拆分文件：{fileName}");
                            var outputFiles = Helpers.TableSplitHelper4Excel.Split(fileName, headerRows, maxRows);
                            foreach (var outputFile in outputFiles)
                            {
                                Log($"拆分进度：{outputFile}");
                            }
                            Log($"完成拆分文件：{fileName}");
                        }
                        break;
                    case ".csv":
                        {
                            Log("------------------------------------");
                            Log($"开始拆分文件：{fileName}");
                            var outputFiles = Helpers.TableSplitHelper4Csv.Split(fileName, headerRows, maxRows);
                            foreach (var outputFile in outputFiles)
                            {
                                Log($"拆分进度：{outputFile}");
                            }
                            Log($"完成拆分文件：{fileName}");
                        }
                        break;
                    default:
                        Log($"{fileName} 不是支持的文件类型，跳过");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"{fileName} 拆分失败：{ex.Message}");
            }
        }

        private void Log(string message)
        {
            _txtLog.Invoke(new Action(() =>
            {
                _txtLog.AppendText($"【{DateTime.Now:yyyyMMddHHmmss}】{message}\r\n");
            }));
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|csv文件(*.csv)|*.csv|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenExcels(openDlg.FileNames.ToList());
        }

        private async void BtnSplit_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                MessageBox.Show("请先添加要拆分的文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var headerRows = (int)_numHeaderRow.Value;
            var maxRows = (int)_numMaxRows.Value;

            var btn = sender as Button;
            if (btn != null) btn.Enabled = false;

            try
            {
                await Task.Run(() =>
                {
                    foreach (var fileName in _inputFileList)
                    {
                        ProcessFile(fileName, headerRows, maxRows);
                    }
                });

                Log($"全部拆分完成");
            }
            finally
            {
                if (btn != null) btn.Enabled = true;
            }
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

            var lbl = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "表头行数："
            };
            _numHeaderRow = new NumericUpDown
            {
                Location = new Point(lbl.Right, lbl.Top - 2),
                Maximum = 100,
                Minimum = 0,
                Parent = this,
                TextAlign = HorizontalAlignment.Right,
                Value = 1,
                Width = 60
            };
            lbl = new Label
            {
                AutoSize = true,
                Location = new Point(_numHeaderRow.Right + Config.ControlPadding, lbl.Top),
                Parent = this,
                Text = "最大数据行数："
            };
            _numMaxRows = new NumericUpDown
            {
                Location = new Point(lbl.Right, lbl.Top - 2),
                Maximum = 100000,
                Minimum = 1,
                Parent = this,
                TextAlign = HorizontalAlignment.Right,
                Value = 100,
                Width = 100
            };

            var btnSplit = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, _numHeaderRow.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "开始拆分"
            };
            btnSplit.Click += BtnSplit_Click;

            var top = btnSplit.Bottom + Config.ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F),
                Location = new Point(Config.ControlMargin, top),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - top),
                WordWrap = false
            };
        }
        #endregion
    }
}
