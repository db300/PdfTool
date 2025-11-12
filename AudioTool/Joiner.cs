using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioTool
{
    /// <summary>
    /// 连接器(多个片段按顺序简单地首尾连接在一起)
    /// </summary>
    public partial class Joiner : UserControl
    {
        #region constructor
        public Joiner()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TextBox _txtFileList;
        private TextBox _txtLog;
        private Button _btnJoin;
        #endregion

        #region method
        private void AppendLog(string message)
        {
            if (_txtLog.InvokeRequired)
            {
                _txtLog.Invoke(new Action(() => _txtLog.AppendText(message)));
            }
            else
            {
                _txtLog.AppendText(message);
            }
        }

        private void JoinAudioFiles(System.Collections.Generic.List<string> inputFileList)
        {
            try
            {
                AppendLog($"开始拼接 {inputFileList.Count} 个音频文件...\r\n");

                // 获取FFmpeg路径
                var ffmpegDir = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "ffmpeg");
                var ffmpegExe = Path.Combine(ffmpegDir, "ffmpeg.exe");

                if (!File.Exists(ffmpegExe))
                {
                    AppendLog($"错误：找不到FFmpeg程序 {ffmpegExe}\r\n");
                    return;
                }

                // 创建concat文件列表（使用不含BOM的UTF-8编码）
                var tempDir = Path.GetTempPath();
                var concatFilePath = Path.Combine(tempDir, $"mp3_concat_{Guid.NewGuid()}.txt");
                var utf8NoBom = new UTF8Encoding(false);
                using (var writer = new StreamWriter(concatFilePath, false, utf8NoBom))
                {
                    foreach (var file in inputFileList)
                    {
                        writer.WriteLine($"file '{file}'");
                    }
                }

                // 构建输出文件路径
                var outputFile = Path.Combine(Path.GetDirectoryName(inputFileList[0]), $"merged-{Guid.NewGuid()}.mp3");
                //AppendLog($"输出文件：{outputFile}\r\n");

                // 构建FFmpeg命令
                var arguments = $"-f concat -safe 0 -i \"{concatFilePath}\" -c copy \"{outputFile}\"";

                // 执行FFmpeg
                var processInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ffmpegExe,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(processInfo))
                {
                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        AppendLog($"拼接成功！输出文件：{outputFile}\r\n");
                    }
                    else
                    {
                        AppendLog($"拼接失败！错误信息：\r\n{error}\r\n");
                    }
                }

                // 清理临时文件
                try
                {
                    File.Delete(concatFilePath);
                }
                catch { }
            }
            catch (Exception ex)
            {
                AppendLog($"异常错误：{ex.Message}\r\n");
            }
            finally
            {
                if (_btnJoin.InvokeRequired)
                {
                    _btnJoin.Invoke(new Action(() => _btnJoin.Enabled = true));
                }
                else
                {
                    _btnJoin.Enabled = true;
                }
            }
        }
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "音频文件(*.mp3)|*.mp3|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var files = openDlg.FileNames.ToList();
            foreach (var fileName in files)
            {
                _txtFileList.AppendText($"{fileName}\r\n");
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            var inputFileList = _txtFileList.Lines.ToList();
            inputFileList.RemoveAll(string.IsNullOrWhiteSpace);
            if (inputFileList.Count == 0)
            {
                AppendLog("未添加需要拼接的音频文件\r\n");
                return;
            }
            _btnJoin.Enabled = false;
            Task.Run(() => JoinAudioFiles(inputFileList));
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
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11),
                Location = new Point(Config.ControlMargin, ClientSize.Height - Config.ControlMargin - 200),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, 200),
                WordWrap = false
            };

            _btnJoin = new Button
            {
                AutoSize = true,
                Location = new Point(btnAddFile.Right + Config.ControlPadding, Config.ControlMargin),
                Parent = this,
                Text = "开始拼接"
            };
            _btnJoin.Click += BtnJoin_Click;

            var top = btnAddFile.Bottom + Config.ControlPadding;
            _txtFileList = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11),
                Location = new Point(btnAddFile.Left, top),
                Multiline = true,
                Parent = this,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, _txtLog.Top - Config.ControlPadding - top),
                WordWrap = false
            };
        }
        #endregion
    }
}
