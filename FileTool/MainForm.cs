using FileTool.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FileTool
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
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/dlhpml59eyue8qls";
        #endregion

        #region event handler
        private void Lbl_LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Feedback);
        }

        private void Lbl_LinkClicked2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Readme);
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Appreciate);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            ClientSize = new Size(1000, 800);
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"文件工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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
            lbl1.LinkClicked += Lbl_LinkClicked1;
            lbl1.Location = new Point(panelFoot.ClientSize.Width - 10 - lbl1.Width, (panelFoot.ClientSize.Height - lbl1.Height) / 2);

            var lbl2 = new LinkLabel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Parent = panelFoot,
                Text = "使用说明",
            };
            lbl2.LinkClicked += Lbl_LinkClicked2;
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
            pic.Click += Pic_Click;
            // 创建并设置ToolTip
            var toolTip = new ToolTip();
            toolTip.SetToolTip(pic, "点击进行赞赏");

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Parent = this
            };
            tabControl.BringToFront();
            tabControl.TabPages.AddRange(new TabPage[]
            {
                new TabPage("批量移动") { BorderStyle = BorderStyle.None, Name = "tpBatchMover" },
                new TabPage("批量改名") { BorderStyle = BorderStyle.None, Name = "tpBatchRenamer" },
                new TabPage("精确查找") { BorderStyle = BorderStyle.None, Name = "tpExactMatchFinder" },
                //new TabPage("txt文件合并") { BorderStyle = BorderStyle.None, Name = "tpTxtMerger" }
            });
            tabControl.TabPages["tpBatchMover"].Controls.Add(new BatchMover { Dock = DockStyle.Fill });
            tabControl.TabPages["tpBatchRenamer"].Controls.Add(new BatchRenamer { Dock = DockStyle.Fill });
            tabControl.TabPages["tpExactMatchFinder"].Controls.Add(new ExactMatchFinder { Dock = DockStyle.Fill });
            //tabControl.TabPages["tpTxtMerger"].Controls.Add(new TxtMerger { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
