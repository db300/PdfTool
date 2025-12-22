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

                DataConvertHelper.ConvertExcelToJson(fileName, outputPath);
                Log($"完成转换文件：{fileName}");
                Log($"输出路径：{outputPath}");
            }
            catch (Exception ex)
            {
                Log($"{fileName} 转换失败：{ex.Message}");
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

            _btnConvert = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, Config.ControlMargin),
                Parent = this,
                Text = "开始转换"
            };
            _btnConvert.Click += BtnConvert_Click;

            var top = btnAddFile.Bottom + Config.ControlPadding;
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
