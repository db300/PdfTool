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

namespace PdfEditor
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
        private TabControl _tabControl;
        #endregion

        #region event handler
        private void BtnRotate_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = false };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var tabPage = new TabPage($"旋转页面 - {Path.GetFileName(openDlg.FileName)}");
            var pageRotator = new Modules.PageRotator { Dock = DockStyle.Fill };
            pageRotator.OpenPdf(openDlg.FileName);
            tabPage.Controls.Add(pageRotator);
            _tabControl.TabPages.Add(tabPage);
            _tabControl.SelectedTab = tabPage;
        }
        #endregion

        #region ui
        private void InitUi()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF编辑器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Parent = this
            };
            var tabPage4Home = new TabPage("主页");
            _tabControl.TabPages.Add(tabPage4Home);

            var btnRotate = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = tabPage4Home,
                Text = "旋转页面"
            };
            btnRotate.Click += BtnRotate_Click;
        }
        #endregion
    }
}
