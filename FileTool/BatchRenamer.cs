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

namespace FileTool
{
    /// <summary>
    /// 批量改名
    /// </summary>
    public partial class BatchRenamer : UserControl
    {
        #region constructor
        public BatchRenamer()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private TextBox _txtDesExt;
        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnAddFiles_Click(object sender, EventArgs e)
        {
            var fileDlg = new OpenFileDialog { Multiselect = true, Title = "选择文件", Filter = "所有文件|*.*" };
            if (fileDlg.ShowDialog() != DialogResult.OK) return;
            _inputFileList.Clear();
            _inputFileList.AddRange(fileDlg.FileNames);
            _txtLog.AppendText($"已添加{_inputFileList.Count}个文件\r\n");
        }

        private void BtnRenameExt_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                _txtLog.AppendText("未添加文件\r\n");
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtDesExt.Text))
            {
                _txtLog.AppendText("未输入目标扩展名\r\n");
                return;
            }
            var desExt = _txtDesExt.Text;
            if (!desExt.StartsWith(".")) desExt = $".{desExt}";
            var successCount = 0;
            foreach (var file in _inputFileList)
            {
                var desFile = Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}{desExt}");
                try
                {
                    File.Move(file, desFile);
                    successCount++;
                    _txtLog.AppendText($"重命名成功：{file} => {desFile}\r\n");
                }
                catch (Exception ex)
                {
                    _txtLog.AppendText($"重命名失败：{file} => {desFile}，{ex.Message}\r\n");
                }
            }
            _txtLog.AppendText($"共{_inputFileList.Count}个文件，成功{successCount}个\r\n");
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var btnAddFiles = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFiles.Click += BtnAddFiles_Click;

            var btnRenameExt = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddFiles.Bottom + Config.ControlPadding),
                Parent = this,
                Text = "批量修改扩展名"
            };
            btnRenameExt.Click += BtnRenameExt_Click;
            _txtDesExt = new TextBox
            {
                Location = new Point(btnRenameExt.Right + Config.ControlPadding, btnRenameExt.Top + 1),
                Parent = this,
                Text = ".xxx"
            };

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(Config.ControlMargin, btnRenameExt.Bottom + Config.ControlPadding),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - btnRenameExt.Bottom - Config.ControlPadding)
            };
        }
        #endregion
    }
}
