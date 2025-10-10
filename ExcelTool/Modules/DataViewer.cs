using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExcelTool.Modules
{
    /// <summary>
    /// 数据预览器
    /// </summary>
    public partial class DataViewer : UserControl, IExcelHandler
    {
        #region constructor
        public DataViewer()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private Dictionary<string, DataTable> _sheetTables;
        private ComboBox _cmbSheets;
        private DataGridView _dataGridView;
        private TextBox _txtLog;
        #endregion

        #region method
        public void OpenExcels(List<string> files)
        {
            //_txtLog.Clear();
            _inputFileList.Clear();
            _inputFileList.AddRange(files);
            foreach (var fileName in _inputFileList)
            {
                _txtLog.AppendText($"【{DateTime.Now:yyyyMMddHHmmss}】{fileName}\r\n");
                _sheetTables = Helpers.DataReadHelper.ReadExcelToDataTable(fileName);
                _txtLog.AppendText($"【{_sheetTables.Count}】个工作表\r\n");
                foreach (var item in _sheetTables)
                {
                    _txtLog.AppendText($"{item.Key}: 行数: {item.Value.Rows.Count}, 列数: {item.Value.Columns.Count}\r\n");
                }
                _cmbSheets.Items.Clear();
                _cmbSheets.Items.AddRange(_sheetTables.Keys.ToArray());
                if (_cmbSheets.Items.Count > 0) _cmbSheets.SelectedIndex = 0;
                break;
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenExcels(openDlg.FileNames.ToList());
        }

        private void CmbSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbSheets.SelectedItem != null && _sheetTables != null)
            {
                var sheetName = _cmbSheets.Text;
                _dataGridView.DataSource = _sheetTables[sheetName];
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
                Text = "打开文件"
            };
            btnAddFile.Click += BtnAddFile_Click;

            _cmbSheets = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, Config.ControlMargin + 2),
                Parent = this,
                Width = 200
            };
            _cmbSheets.SelectedIndexChanged += CmbSheets_SelectedIndexChanged;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F),
                Location = new Point(Config.ControlMargin, ClientSize.Height - Config.ControlMargin - 200 - Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 200),
                WordWrap = false
            };

            var top = btnAddFile.Bottom + Config.ControlPadding;
            _dataGridView = new DataGridView
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, top),
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, _txtLog.Top - Config.ControlPadding - top),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Parent = this
            };
        }
        #endregion
    }
}
