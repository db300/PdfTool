using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileTool
{
    /// <summary>
    /// 文本文件编码转换器
    /// </summary>
    public partial class TextEncodingConverter : UserControl
    {
        #region constructor
        public TextEncodingConverter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _inputFileList = new List<string>();
        private readonly Dictionary<string, Encoding> _detectedEncodings = new Dictionary<string, Encoding>();
        private ComboBox _cbSourceEncoding;
        private ComboBox _cbTargetEncoding;
        private CheckBox _ckbRemoveBom;
        private CheckBox _ckbAutoDetect;
        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnAddFiles_Click(object sender, EventArgs e)
        {
            var fileDlg = new OpenFileDialog 
            { 
                Multiselect = true, 
                Title = "选择要转换的文本文件",
                Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*"
            };
            if (fileDlg.ShowDialog() != DialogResult.OK) return;
            _inputFileList.Clear();
            _detectedEncodings.Clear();
            _inputFileList.AddRange(fileDlg.FileNames);
            _txtLog.AppendText($"已添加 {_inputFileList.Count} 个文件\r\n");
        }

        private void BtnDetectEncoding_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                _txtLog.AppendText("未添加文件\r\n");
                return;
            }

            _txtLog.AppendText("开始检测文件编码...\r\n");
            var total = _inputFileList.Count;

            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                for (int i = 0; i < _inputFileList.Count; i++)
                {
                    var file = _inputFileList[i];
                    try
                    {
                        var detectedEncoding = DetectFileEncoding(file);
                        _detectedEncodings[file] = detectedEncoding;
                        var encodingName = GetEncodingName(detectedEncoding);
                        background.ReportProgress(i + 1, $"检测完成：{Path.GetFileName(file)} - {encodingName}");
                    }
                    catch (Exception ex)
                    {
                        background.ReportProgress(i + 1, $"检测失败：{Path.GetFileName(file)}，原因：{ex.Message}");
                    }
                }
            };
            background.ProgressChanged += (ss, ee) =>
            {
                _txtLog.AppendText($"【{ee.ProgressPercentage} / {total}】{ee.UserState}\r\n");
            };
            background.RunWorkerCompleted += (ss, ee) =>
            {
                _txtLog.AppendText($"编码检测完成\r\n");
            };
            background.RunWorkerAsync();
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            if (_inputFileList.Count == 0)
            {
                _txtLog.AppendText("未添加文件\r\n");
                return;
            }
            if (_cbTargetEncoding.SelectedIndex < 0)
            {
                _txtLog.AppendText("未选择目标编码\r\n");
                return;
            }

            if (!_ckbAutoDetect.Checked && _cbSourceEncoding.SelectedIndex < 0)
            {
                _txtLog.AppendText("未选择源编码且未启用自动检测\r\n");
                return;
            }

            var targetEncoding = GetEncodingByName((string)_cbTargetEncoding.SelectedItem);
            var removeBom = _ckbRemoveBom.Checked;
            var autoDetect = _ckbAutoDetect.Checked;
            var sourceEncodingName = _cbSourceEncoding.SelectedIndex >= 0 ? (string)_cbSourceEncoding.SelectedItem : null;

            _txtLog.AppendText("开始转换文件...\r\n");
            var successCount = 0;
            var total = _inputFileList.Count;

            var background = new BackgroundWorker { WorkerReportsProgress = true };
            background.DoWork += (ww, ee) =>
            {
                for (int i = 0; i < _inputFileList.Count; i++)
                {
                    var file = _inputFileList[i];
                    try
                    {
                        Encoding sourceEncoding;
                        if (autoDetect && _detectedEncodings.ContainsKey(file))
                        {
                            sourceEncoding = _detectedEncodings[file];
                        }
                        else if (autoDetect)
                        {
                            sourceEncoding = DetectFileEncoding(file);
                        }
                        else
                        {
                            sourceEncoding = GetEncodingByName(sourceEncodingName);
                        }

                        string content = File.ReadAllText(file, sourceEncoding);
                        byte[] targetBytes = removeBom 
                            ? targetEncoding.GetBytes(content) 
                            : targetEncoding.GetPreamble().Concat(targetEncoding.GetBytes(content)).ToArray();
                        File.WriteAllBytes(file, targetBytes);
                        background.ReportProgress(i + 1, $"转换成功：{Path.GetFileName(file)}");
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        background.ReportProgress(i + 1, $"转换失败：{Path.GetFileName(file)}，原因：{ex.Message}");
                    }
                }
            };
            background.ProgressChanged += (ss, ee) =>
            {
                _txtLog.AppendText($"【{ee.ProgressPercentage} / {total}】{ee.UserState}\r\n");
            };
            background.RunWorkerCompleted += (ss, ee) =>
            {
                _txtLog.AppendText($"转换完成，共 {total} 个文件，成功 {successCount} 个\r\n");
            };
            background.RunWorkerAsync();
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            _txtLog.Clear();
        }

        private void CkbAutoDetect_CheckedChanged(object sender, EventArgs e)
        {
            _cbSourceEncoding.Enabled = !_ckbAutoDetect.Checked;
        }
        #endregion

        #region helper
        private Encoding DetectFileEncoding(string filePath)
        {
            byte[] buffer = new byte[5];
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                file.Read(buffer, 0, 5);
            }

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                return Encoding.UTF8;

            if (buffer[0] == 0xff && buffer[1] == 0xfe && buffer[2] == 0 && buffer[3] == 0)
                return Encoding.UTF32;

            if (buffer[0] == 0xff && buffer[1] == 0xfe)
                return Encoding.Unicode;

            if (buffer[0] == 0xfe && buffer[1] == 0xff)
                return Encoding.BigEndianUnicode;

            if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                return Encoding.UTF32;

            return DetectEncoding(filePath);
        }

        private Encoding DetectEncoding(string filePath)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                bytesRead = file.Read(buffer, 0, buffer.Length);
            }

            if (bytesRead == 0) return Encoding.UTF8;

            int nullCount = 0;
            for (int i = 0; i < bytesRead; i++)
            {
                if (buffer[i] == 0)
                    nullCount++;
            }

            if (nullCount > bytesRead / 4)
                return Encoding.Unicode;

            try
            {
                var isUtf8 = IsValidUtf8(buffer, bytesRead);
                if (isUtf8)
                    return Encoding.UTF8;
            }
            catch { }

            try
            {
                Encoding.GetEncoding("GBK").GetString(buffer, 0, bytesRead);
                return Encoding.GetEncoding("GBK");
            }
            catch { }

            if (IsAscii(buffer, bytesRead))
                return Encoding.ASCII;

            return Encoding.UTF8;
        }

        private bool IsValidUtf8(byte[] buffer, int length)
        {
            int i = 0;
            while (i < length)
            {
                if ((buffer[i] & 0x80) == 0)
                {
                    i += 1;
                }
                else if ((buffer[i] & 0xE0) == 0xC0)
                {
                    if (i + 1 >= length || (buffer[i + 1] & 0xC0) != 0x80)
                        return false;
                    i += 2;
                }
                else if ((buffer[i] & 0xF0) == 0xE0)
                {
                    if (i + 2 >= length || (buffer[i + 1] & 0xC0) != 0x80 || (buffer[i + 2] & 0xC0) != 0x80)
                        return false;
                    i += 3;
                }
                else if ((buffer[i] & 0xF8) == 0xF0)
                {
                    if (i + 3 >= length || (buffer[i + 1] & 0xC0) != 0x80 || (buffer[i + 2] & 0xC0) != 0x80 || (buffer[i + 3] & 0xC0) != 0x80)
                        return false;
                    i += 4;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsAscii(byte[] buffer, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (buffer[i] > 127)
                    return false;
            }
            return true;
        }

        private string GetEncodingName(Encoding encoding)
        {
            if (encoding.Equals(Encoding.UTF8))
                return "UTF-8";
            if (encoding.CodePage == Encoding.Unicode.CodePage)
                return "UTF-16 LE";
            if (encoding.CodePage == Encoding.BigEndianUnicode.CodePage)
                return "UTF-16 BE";
            if (encoding.CodePage == Encoding.UTF32.CodePage)
                return "UTF-32 LE";
            if (encoding.CodePage == Encoding.ASCII.CodePage)
                return "ASCII";
            if (encoding.CodePage == 936)
                return "ANSI (GBK)";
            if (encoding.CodePage == 20936)
                return "ANSI (GB2312)";
            return encoding.EncodingName;
        }

        private Encoding GetEncodingByName(string encodingName)
        {
            switch (encodingName)
            {
                case "UTF-8":
                    return Encoding.UTF8;
                case "UTF-8 (no BOM)":
                    return new UTF8Encoding(false);
                case "UTF-16 LE":
                    return Encoding.Unicode;
                case "UTF-16 BE":
                    return Encoding.BigEndianUnicode;
                case "UTF-32 LE":
                    return Encoding.UTF32;
                case "ANSI (GBK)":
                    return Encoding.GetEncoding("GBK");
                case "ANSI (GB2312)":
                    return Encoding.GetEncoding("GB2312");
                case "ASCII":
                    return Encoding.ASCII;
                default:
                    return Encoding.UTF8;
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var group4Setting = new GroupBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 145),
                Text = "编码转换设置"
            };
            InitUi4SettingGroup(group4Setting);

            var top = group4Setting.Bottom + Config.ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11F, GraphicsUnit.Point),
                Location = new Point(Config.ControlMargin, top),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - top),
                WordWrap = false
            };
        }

        private void InitUi4SettingGroup(GroupBox groupBox)
        {
            var btnAddFiles = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin + Config.ControlPadding),
                Parent = groupBox,
                Text = "添加文件"
            };
            btnAddFiles.Click += BtnAddFiles_Click;

            _ckbAutoDetect = new CheckBox
            {
                AutoSize = true,
                Location = new Point(btnAddFiles.Right + Config.ControlPadding * 2, btnAddFiles.Top + 3),
                Parent = groupBox,
                Text = "自动检测源编码"
            };
            _ckbAutoDetect.CheckedChanged += CkbAutoDetect_CheckedChanged;

            var btnDetectEncoding = new Button
            {
                AutoSize = true,
                Location = new Point(_ckbAutoDetect.Right + Config.ControlPadding * 2, btnAddFiles.Top),
                Parent = groupBox,
                Text = "检测编码"
            };
            btnDetectEncoding.Click += BtnDetectEncoding_Click;

            var lblSourceEncoding = new Label
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnAddFiles.Bottom + Config.ControlPadding + 3),
                Parent = groupBox,
                Text = "源编码："
            };

            _cbSourceEncoding = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lblSourceEncoding.Right + Config.ControlPadding, btnAddFiles.Bottom + Config.ControlPadding + 1),
                Parent = groupBox,
                Width = 120
            };
            _cbSourceEncoding.Items.AddRange(new object[]
            {
                "UTF-8",
                "UTF-8 (no BOM)",
                "UTF-16 LE",
                "UTF-16 BE",
                "UTF-32 LE",
                "ANSI (GBK)",
                "ANSI (GB2312)",
                "ASCII"
            });
            _cbSourceEncoding.SelectedIndex = 0;

            var lblTargetEncoding = new Label
            {
                AutoSize = true,
                Location = new Point(_cbSourceEncoding.Right + Config.ControlPadding * 2, _cbSourceEncoding.Top + 3),
                Parent = groupBox,
                Text = "目标编码："
            };

            _cbTargetEncoding = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(lblTargetEncoding.Right + Config.ControlPadding, _cbSourceEncoding.Top + 1),
                Parent = groupBox,
                Width = 120
            };
            _cbTargetEncoding.Items.AddRange(new object[]
            {
                "UTF-8",
                "UTF-8 (no BOM)",
                "UTF-16 LE",
                "UTF-16 BE",
                "UTF-32 LE",
                "ANSI (GBK)",
                "ANSI (GB2312)",
                "ASCII"
            });
            _cbTargetEncoding.SelectedIndex = 0;

            var btnConvert = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, _cbSourceEncoding.Bottom + Config.ControlPadding),
                Parent = groupBox,
                Text = "开始转换"
            };
            btnConvert.Click += BtnConvert_Click;

            _ckbRemoveBom = new CheckBox
            {
                AutoSize = true,
                Location = new Point(btnConvert.Right + Config.ControlPadding, btnConvert.Top + 3),
                Parent = groupBox,
                Text = "移除BOM"
            };

            var btnClearLog = new Button
            {
                AutoSize = true,
                Location = new Point(_ckbRemoveBom.Right + Config.ControlPadding, btnConvert.Top),
                Parent = groupBox,
                Text = "清除日志"
            };
            btnClearLog.Click += BtnClearLog_Click;
        }
        #endregion
    }
}
