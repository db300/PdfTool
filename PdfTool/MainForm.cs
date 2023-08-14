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

        #region ui
        private void InitUi()
        {
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"PDF工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

            var tabControl = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(10, 10),
                Parent = this,
                Size = new Size(ClientSize.Width - 10 * 2, ClientSize.Height - 10 * 2)
            };
            tabControl.TabPages.AddRange(new TabPage[]
            {
                new TabPage("PDF拆分") { BorderStyle = BorderStyle.None },
                new TabPage("PDF合并") { BorderStyle = BorderStyle.None },
                new TabPage("PDF图片提取") { BorderStyle = BorderStyle.None, Name = "tpPdfImageExtracter" }
            });

            tabControl.TabPages["tpPdfImageExtracter"].Controls.Add(new PdfImageExtracter { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
