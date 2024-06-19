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
    /// TXT文件合并器
    /// </summary>
    public partial class TxtMerger : UserControl
    {
        #region constructor
        public TxtMerger()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private readonly List<string> _fileList = new List<string>();
        private TextBox _txtLog;
        #endregion

        #region event handler
        private void BtnScanFolder_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog { Description = "选择主文件夹", RootFolder = Environment.SpecialFolder.Desktop, SelectedPath = @"C:\" };
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            var selectedPath = folderDlg.SelectedPath;
            var files = ScanTxtFile(selectedPath);
            _fileList.Clear();
            _fileList.AddRange(files);
            _txtLog.Text = string.Join("\r\n", _fileList);
        }

        private void BtnMerge_Click(object sender, EventArgs e)
        {
            
            var saveDlg = new SaveFileDialog { DefaultExt = "txt" };
            if (saveDlg.ShowDialog() != DialogResult.OK) return;
            MergeTxtFiles(_fileList, saveDlg.FileName);
            /*
            // 获取所有的文件夹
            var dirs = Directory.GetDirectories(rootFolder).OrderBy(d => d);

            // 创建或清空目标文件
            File.WriteAllText(outputFile, string.Empty);

            // 遍历每个文件夹
            foreach (var dir in dirs)
            {
                // 获取文件夹内的txt文件
                var txtFiles = string.IsNullOrWhiteSpace(specialFileName)
                    ? Directory.GetFiles(dir, "*.txt")
                    : Directory.GetFiles(dir, specialFileName);
                // 遍历每个txt文件
                foreach (var txtFile in txtFiles)
                {
                    // 读取txt文件并将内容追加到目标文件
                    var content = File.ReadAllText(txtFile);
                    File.AppendAllText(outputFile, content);
                }
            }
            */
        }
        #endregion

        #region method
        private void MergeTxtFiles(List<string> inputFiles, string outputFile)
        {
            // 创建或清空目标文件
            File.WriteAllText(outputFile, string.Empty);
            // 遍历每个txt文件
            foreach (var txtFile in inputFiles)
            {
                // 读取txt文件并将内容追加到目标文件
                var content = File.ReadAllText(txtFile);
                File.AppendAllText(outputFile, content);
            }
        }

        private List<string> ScanTxtFile(string path)
        {
            var files = new List<string>();
            files.AddRange(Directory.GetFiles(path, "*.txt"));
            var dirs = Directory.GetDirectories(path).OrderBy(d => d);
            foreach (var dir in dirs)
            {
                files.AddRange(ScanTxtFile(dir));
            }
            return files;
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var btnScanFolder = new Button
            {
                Location = new Point(10, 10),
                Parent = this,
                Text = "扫描文件夹"
            };
            btnScanFolder.Click += BtnScanFolder_Click;

            var btnMerge = new Button
            {
                Location = new Point(btnScanFolder.Right + 10, 10),
                Parent = this,
                Text = "合并"
            };
            btnMerge.Click += BtnMerge_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(10, btnScanFolder.Bottom + 10),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - 20, ClientSize.Height - 20 - btnScanFolder.Bottom),
                WordWrap = false
            };
        }
        #endregion
    }
}
