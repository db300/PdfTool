using AudioTool.Properties;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AudioTool
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
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/hcws2banecof1sab";
        #endregion

        #region event handler
        #endregion

        #region ui
        private void InitUi()
        {
            ClientSize = new Size(1000, 800);
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"音频工具 v{Application.ProductVersion}";

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Parent = this,
                TabPages =
                {
                    new TabPage("音频拼接") { BorderStyle = BorderStyle.None, Controls = { new Joiner { Dock = DockStyle.Fill } } }
                }
            };

            //状态栏
            var statusStrip = new StatusStrip { ShowItemToolTips = true };

            // 左侧赞赏按钮
            var iconButton = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Resources.redheart,
                ToolTipText = "点击进行赞赏"
            };
            iconButton.Click += (s, e) => { Process.Start(new ProcessStartInfo { FileName = Url4Appreciate, UseShellExecute = true }); };

            // 占位符（弹性填充）
            var springLabel = new ToolStripStatusLabel { Spring = true };

            // 右侧超链接
            var linkLabel0 = new ToolStripStatusLabel
            {
                IsLink = true,
                Text = "使用说明",
                ToolTipText = Url4Readme,
                TextAlign = ContentAlignment.MiddleRight
            };
            linkLabel0.Click += (s, e) => { Process.Start(new ProcessStartInfo { FileName = Url4Readme, UseShellExecute = true }); };

            var linkLabel1 = new ToolStripStatusLabel
            {
                IsLink = true,
                Text = "问题反馈",
                ToolTipText = Url4Feedback,
                TextAlign = ContentAlignment.MiddleRight
            };
            linkLabel1.Click += (s, e) => { Process.Start(new ProcessStartInfo { FileName = Url4Feedback, UseShellExecute = true }); };

            statusStrip.Items.Add(iconButton);
            statusStrip.Items.Add(springLabel);
            statusStrip.Items.Add(linkLabel0);
            statusStrip.Items.Add(linkLabel1);
            Controls.Add(statusStrip);
        }
        #endregion
    }
}
