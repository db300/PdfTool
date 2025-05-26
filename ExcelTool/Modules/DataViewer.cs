using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExcelTool.Modules
{
    /// <summary>
    /// 数据预览器
    /// </summary>
    public partial class DataViewer : UserControl
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
        private TextBox _txtLog;
        #endregion

        #region method
        public void OpenFile(List<string> files)
        {
            _txtLog.Clear();
            _inputFileList.Clear();
            _inputFileList.AddRange(files);
            foreach (var fileName in _inputFileList)
            {
                _txtLog.AppendText($"【{DateTime.Now:yyyyMMddHHmmss}】{fileName}\r\n");
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            OpenFile(openDlg.FileNames.ToList());
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
