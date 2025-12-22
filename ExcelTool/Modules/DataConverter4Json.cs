using ExcelTool.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelTool.Modules
{
    /// <summary>
    /// 数据转换器(数据转json)
    /// </summary>
    public partial class DataConverter4Json : UserControl, IExcelHandler
    {
        #region constructor
        public DataConverter4Json()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private TextBox _txtLog;
        private Button _btnConvert;
        private CheckBox _chkSkipEmptyRows;
        private TextBox _txtColumnMapping;
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

        private void ProcessFile(string fileName)
        {
            try
            {
                var ext = Path.GetExtension(fileName).ToLower();
                if (ext != ".xls" && ext != ".xlsx")
                {
                    Log($"{fileName} 不是支持的Excel文件，跳过");
                    return;
                }

                Log($"------------------------------------");
                Log($"开始转换文件：{fileName}");

                var outputPath = Path.Combine(
                    Path.GetDirectoryName(fileName),
                    Path.GetFileNameWithoutExtension(fileName) + ".json"
                );

                var skipEmptyRows = _chkSkipEmptyRows.Checked;
                var columnMapping = ParseColumnMapping(_txtColumnMapping.Text);

                DataConvertHelper.ConvertExcelToJson(fileName, outputPath, skipEmptyRows, columnMapping);
                Log($"完成转换文件：{fileName}");
                Log($"输出路径：{outputPath}");
            }
            catch (Exception ex)
            {
                Log($"{fileName} 转换失败：{ex.Message}");
            }
        }

        private Dictionary<string, string> ParseColumnMapping(string mappingText)
        {
            if (string.IsNullOrWhiteSpace(mappingText))
            {
                return null;
            }

            var mapping = new Dictionary<string, string>();
            var pairs = mappingText.Split(new[] { ';', '；' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var parts = pair.Split(new[] { '=', '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        mapping[key] = value;
                    }
                }
            }

            return mapping.Count > 0 ? mapping : null;
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
            var openDlg = new OpenFileDialog
            {
                Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件(*.*)|*.*",
                Multiselect = true
            };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenExcels(openDlg.FileNames.ToList());
        }

        private async void BtnConvert_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                MessageBox.Show("请先添加要转换的Excel文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _btnConvert.Enabled = false;

            try
            {
                await Task.Run(() =>
                {
                    foreach (var fileName in _inputFileList)
                    {
                        ProcessFile(fileName);
                    }
                });

                Log($"全部转换完成");
            }
            finally
            {
                _btnConvert.Enabled = true;
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

            _chkSkipEmptyRows = new CheckBox
            {
                AutoSize = true,
                Checked = true,
                Location = new Point(Config.ControlMargin, btnAddFile.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "跳过空行"
            };

            var lblMapping = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, _chkSkipEmptyRows.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "列名映射："
            };

            _txtColumnMapping = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(lblMapping.Right + 5, lblMapping.Top - 2),
                Parent = this,
                Size = new Size(ClientSize.Width - lblMapping.Right - 5 - Config.ControlMargin, 23)
            };

            var lblMappingTip = new Label
            {
                AutoSize = true,
                ForeColor = Color.Gray,
                Location = new Point(_txtColumnMapping.Left, _txtColumnMapping.Bottom + 3),
                Parent = this,
                Text = "格式：原列名1=新key1;原列名2=新key2 (可选，留空则使用原列名)"
            };

            _btnConvert = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, lblMappingTip.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "开始转换"
            };
            _btnConvert.Click += BtnConvert_Click;

            var top = _btnConvert.Bottom + Config.ControlPadding;
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
