using PdfEditor.Properties;
using System;
using System.Drawing;
using System.IO;
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
        private const string Url4Appreciate = "https://www.yuque.com/lengda/eq8cm6/rylia4";
        private const string Url4Feedback = "https://www.yuque.com/lengda/eq8cm6/ezwik4";
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/dts6ubl9o0pr51u5";

        private TabControl _tabControl;
        #endregion

        #region method
        private void OpenFile()
        {

        }
        #endregion

        #region event handler
        private void BtnRotate_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = false };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var tabPage = new TabPage($"页面旋转 - {Path.GetFileName(openDlg.FileName)}");
            var pageRotator = new Modules.PageRotator { Dock = DockStyle.Fill };
            pageRotator.OpenPdf(openDlg.FileName);
            tabPage.Controls.Add(pageRotator);
            _tabControl.TabPages.Add(tabPage);
            _tabControl.SelectedTab = tabPage;
        }

        private void BtnSort_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = false };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var tabPage = new TabPage($"页面排序 - {Path.GetFileName(openDlg.FileName)}");
            var pageSorter = new Modules.PageSorter { Dock = DockStyle.Fill };
            pageSorter.OpenPdf(openDlg.FileName);
            tabPage.Controls.Add(pageSorter);
            _tabControl.TabPages.Add(tabPage);
            _tabControl.SelectedTab = tabPage;
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            var folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() != DialogResult.OK) return;
            var tabPage = new TabPage($"PDF浏览 - {folderDlg.SelectedPath}");
            var browser = new Modules.Browser { Dock = DockStyle.Fill };
            browser.ScanPdf(folderDlg.SelectedPath);
            tabPage.Controls.Add(browser);
            _tabControl.TabPages.Add(tabPage);
            _tabControl.SelectedTab = tabPage;
        }
        #endregion

        #region ui
        private void InitUi()
        {
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF编辑器 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var panelFoot = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 25,
                Parent = this
            };
            var lbl1 = new LinkLabel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Parent = panelFoot,
                Text = "问题和需求反馈",
            };
            lbl1.LinkClicked += (sender, ee) => { System.Diagnostics.Process.Start(Url4Feedback); };
            lbl1.Location = new Point(panelFoot.ClientSize.Width - 10 - lbl1.Width, (panelFoot.ClientSize.Height - lbl1.Height) / 2);
            var lbl2 = new LinkLabel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Parent = panelFoot,
                Text = "使用说明",
            };
            lbl2.LinkClicked += (sender, ee) => { System.Diagnostics.Process.Start(Url4Readme); };
            lbl2.Location = new Point(lbl1.Left - 10 - lbl2.Width, (panelFoot.ClientSize.Height - lbl2.Height) / 2);
            var pic = new PictureBox
            {
                Cursor = Cursors.Hand,
                Dock = DockStyle.Left,
                Image = Image.FromStream(new MemoryStream(Resources.redheart)),
                Parent = panelFoot,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 25
            };
            pic.Click += (sender, ee) => { System.Diagnostics.Process.Start(Url4Appreciate); };
            // 创建并设置ToolTip
            var toolTip = new ToolTip();
            toolTip.SetToolTip(pic, "点击进行赞赏");

            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Parent = this
            };
            _tabControl.BringToFront();
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

            var btnSort = new Button
            {
                AutoSize = true,
                Location = new Point(btnRotate.Right + Config.ControlPadding, btnRotate.Top),
                Parent = tabPage4Home,
                Text = "页面排序",
                Visible = false
            };
            btnSort.Click += BtnSort_Click;

            var picLine = new PictureBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.LightGray,
                Location = new Point(Config.ControlMargin, btnSort.Bottom + Config.ControlPadding),
                Parent = tabPage4Home,
                Size = new Size(tabPage4Home.ClientSize.Width - 2 * Config.ControlMargin, 1)
            };

            var btnScan = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, picLine.Bottom + Config.ControlPadding),
                Parent = tabPage4Home,
                Text = "浏览PDF"
            };
            btnScan.Click += BtnScan_Click;
        }
        #endregion
    }
}
