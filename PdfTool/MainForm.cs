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

            AllowDrop = true;
            DragEnter += (sender, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            };
            DragDrop += (sender, e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files.Length == 1)
                    {
                        var file = files[0];
                        if (file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            var pdfSplitter = Controls.Find("tpPdfSplitter", true).FirstOrDefault()?.Controls.OfType<PdfSplitter>().FirstOrDefault();
                            //pdfSplitter?.OpenPdf(file);
                        }
                        else if (file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            var imageImporter = Controls.Find("tpImageImporter", true).FirstOrDefault()?.Controls.OfType<ImageImporter>().FirstOrDefault();
                            //imageImporter?.OpenImage(file);
                        }
                    }
                }
            };
        }
        #endregion

        #region property
        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
        private const string Url4Feedback = "https://www.yuque.com/lengda/eq8cm6/ezwik4?singleDoc# 《需求记录》";
        #endregion

        #region event handler
        private void Lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Appreciate);
        }

        private void Lbl_LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Feedback);
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
                Text = "如果觉得好用，来打赏一下啊~"
            };
            lbl.LinkClicked += Lbl_LinkClicked;

            lbl = new LinkLabel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Parent = this,
                Text = "如果有问题和需求，欢迎来反馈~",
            };
            lbl.LinkClicked += Lbl_LinkClicked1;
            lbl.Location = new Point(ClientSize.Width - 10 - lbl.Width, 10);

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
                new TabPage("PDF表格提取") { BorderStyle = BorderStyle.None, Name = "tpPdfTableExtracter" },
                new TabPage("PDF文本提取") { BorderStyle = BorderStyle.None, Name = "tpPdfTextExtracter" },
                new TabPage("图片导入PDF") { BorderStyle = BorderStyle.None, Name = "tpImageImporter" },
                new TabPage("批量打印") { BorderStyle = BorderStyle.None, Name = "tpPdfPrinter" }
            });

            tabControl.TabPages["tpPdfSplitter"].Controls.Add(new PdfSplitter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfMerger"].Controls.Add(new PdfMerger { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfImager"].Controls.Add(new PdfImager { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfImageExtracter"].Controls.Add(new PdfImageExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfTableExtracter"].Controls.Add(new PdfTableExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfTextExtracter"].Controls.Add(new PdfTextExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpImageImporter"].Controls.Add(new ImageImporter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpPdfPrinter"].Controls.Add(new PdfPrinter { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
