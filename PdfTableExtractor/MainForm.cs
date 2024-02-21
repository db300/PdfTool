using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfTableExtractor
{
    public partial class MainForm : Form
    {
        #region constructor
        public MainForm()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private const int ControlMargin = 20;
        private const int ControlPadding = 12;
        #endregion

        #region event handler
        private void BtnAddFile_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;

            var list = new List<List<string>>();
            foreach (var file in openDlg.FileNames)
            {
                var result = Common.Pdf2Table(file, new List<int>(), new List<int>());
                if (result.Count > 0) list.AddRange(result);
            }
            System.Diagnostics.Debug.WriteLine(list);
            list.RemoveRange(list.Count - 5, 5);
            var unicodeList = new List<string>();
            var chaList = new List<string>();
            foreach (var item in list)
            {
                chaList.Add(item[1].Trim());
                unicodeList.Add(item[2].Split('+')[1].Trim());
                /*
                foreach (var s in item)
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    var ss = s.Trim().Split('\n').ToList();
                    if (ss.Count == 1) unicodeList.Add(ss[0]);
                    else unicodeList.Add(ss[1]);
                }
                */
            }
            System.Diagnostics.Debug.WriteLine(unicodeList);
            var a = string.Join("\r\n", unicodeList);
            var b = string.Join("", chaList);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF表格提取器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var btnAddFile = new Button
            {
                AutoSize = true,
                Location = new Point(ControlMargin, ControlMargin),
                Parent = this,
                Text = "添加文件"
            };
            btnAddFile.Click += BtnAddFile_Click;
        }
        #endregion
    }
}
