using PdfTool.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

            DragEnter += MainForm_DragEnter;
            DragDrop += MainForm_DragDrop;
        }
        #endregion

        #region property
        private const string Url4Appreciate = "https://www.yuque.com/lengda/eq8cm6/rylia4";
        private const string Url4Feedback = "https://www.yuque.com/lengda/eq8cm6/ezwik4";
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/fgfthhr3e53qkszl";
        #endregion

        #region event handler
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();
                var pdfFiles = files.Where(a => a.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)).ToList();
                var tabPage = Controls.OfType<TabControl>().FirstOrDefault().SelectedTab;
                var control = tabPage.Controls[0];
                if (control is IPdfHandler pdfHandler)
                {
                    pdfHandler.OpenPdfs(pdfFiles);
                }
                else if (control is ImageImporter imageImporter)
                {
                    var extList = new List<string> { ".bmp", ".jpg", ".tif", ".png" };
                    var imgFiles = files.Where(a => extList.Contains(Path.GetExtension(a).ToLower())).ToList();
                    imageImporter.OpenImages(imgFiles);
                }
            }
        }

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
            AllowDrop = true;
            ClientSize = new Size(1000, 800);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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
                new TabPage("PDF拆分") { BorderStyle = BorderStyle.None, Name = "tpPdfSplitter" },
                new TabPage("PDF合并") { BorderStyle = BorderStyle.None, Name = "tpPdfMerger" },
                new TabPage("PDF转图") { BorderStyle = BorderStyle.None, Name = "tpPdfImager" },
                new TabPage("PDF图片提取") { BorderStyle = BorderStyle.None, Name = "tpPdfImageExtracter" },
                new TabPage("PDF表格提取") { BorderStyle = BorderStyle.None, Name = "tpPdfTableExtracter" },
                new TabPage("PDF文本提取") { BorderStyle = BorderStyle.None, Name = "tpPdfTextExtracter" },
                new TabPage("PDF页面旋转") { BorderStyle = BorderStyle.None, Name = "tpPdfPageRotator" },
                new TabPage("图片导入PDF") { BorderStyle = BorderStyle.None, Name = "tpImageImporter" },
                new TabPage("批量打印") { BorderStyle = BorderStyle.None, Name = "tpPdfPrinter" },
                new TabPage("PDF保护") { BorderStyle = BorderStyle.None, Name = "tpPdfProtector" },
                //new TabPage("PDF压缩") { BorderStyle = BorderStyle.None, Name = "tpPdfCompressor" },
                new TabPage("PDF修复") { BorderStyle = BorderStyle.None, Name = "tpPdfRepairer" },
                new TabPage("PDF预览") { BorderStyle = BorderStyle.None, Name = "tpPdfPreviewer" }
            });

            tabControl.TabPages["tpPdfSplitter"].Controls.Add(new PdfSplitter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfMerger"].Controls.Add(new PdfMerger { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfImager"].Controls.Add(new PdfImager { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfImageExtracter"].Controls.Add(new PdfImageExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfTableExtracter"].Controls.Add(new PdfTableExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfTextExtracter"].Controls.Add(new PdfTextExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfPageRotator"].Controls.Add(new PageRotator { Dock = DockStyle.Fill });
            tabControl.TabPages["tpImageImporter"].Controls.Add(new ImageImporter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfPrinter"].Controls.Add(new PdfPrinter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfProtector"].Controls.Add(new PdfProtector { Dock = DockStyle.Fill });
            //tabControl.TabPages["tpPdfCompressor"].Controls.Add(new PdfCompressor { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfRepairer"].Controls.Add(new PdfRepairer { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfPreviewer"].Controls.Add(new PdfPreviewer { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
