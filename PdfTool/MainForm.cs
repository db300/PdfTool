using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfTool
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
        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
        #endregion

        #region event handler

        private void Lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Appreciate);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var lbl = new LinkLabel
            {
                AutoSize = true,
                Location = new Point(10, 10),
                Parent = this,
                Text = "如果觉得好用，来打赏一下啊 O(∩_∩)O 哈哈~"
            };
            lbl.LinkClicked += Lbl_LinkClicked;

            var tabControl = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(10, lbl.Bottom + 10),
                Parent = this,
                Size = new Size(ClientSize.Width - 10 * 2, ClientSize.Height - 10 * 2 - lbl.Bottom)
            };
            tabControl.TabPages.AddRange(new TabPage[]
            {
                new TabPage("PDF拆分") { BorderStyle = BorderStyle.None, Name = "tpPdfSplitter" },
                new TabPage("PDF合并") { BorderStyle = BorderStyle.None, Name = "tpPdfMerger" },
                new TabPage("PDF转图") { BorderStyle = BorderStyle.None, Name = "tpPdfImager" },
                new TabPage("PDF图片提取") { BorderStyle = BorderStyle.None, Name = "tpPdfImageExtracter" },
                new TabPage("图片导入PDF") { BorderStyle = BorderStyle.None, Name = "tpImageImporter" }
            });

            tabControl.TabPages["tpPdfSplitter"].Controls.Add(new PdfSplitter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfMerger"].Controls.Add(new PdfMerger { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfImager"].Controls.Add(new PdfImager { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfImageExtracter"].Controls.Add(new PdfImageExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpImageImporter"].Controls.Add(new ImageImporter { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
