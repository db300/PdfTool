using ExcelTool.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExcelTool
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
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/zqfgwqx6g6azmkho";
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
                var excelFiles = files.Where(a =>
                a.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) ||
                a.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                a.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)).ToList();
                var tabPage = Controls.OfType<TabControl>().FirstOrDefault().SelectedTab;
                var control = tabPage.Controls[0];
                if (control is IExcelHandler excelHandler)
                {
                    excelHandler.OpenExcels(excelFiles);
                }
            }
        }

        private void Lbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Appreciate);
        }

        private void Lbl_LinkClicked1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Feedback);
        }

        private void Lbl_LinkClicked2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Readme);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            AllowDrop = true;
            ClientSize = new Size(1200, 1000);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"Excel工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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
                Text = "问题反馈",
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

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Parent = this
            };
            tabControl.BringToFront();
            tabControl.TabPages.AddRange(new TabPage[]
            {
                new TabPage("图片提取") { BorderStyle = BorderStyle.None, Name = "tpImageExtracter" },
                new TabPage("数据提取") { BorderStyle = BorderStyle.None, Name = "tpDataExtracter" },
                new TabPage("数据预览") { BorderStyle = BorderStyle.None, Name = "tpDataViewer" },
                new TabPage("表格拆分") { BorderStyle = BorderStyle.None, Name = "tpTableSplitter"},
            });
            tabControl.TabPages["tpImageExtracter"].Controls.Add(new ImageExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpDataExtracter"].Controls.Add(new DataExtracter { Dock = DockStyle.Fill });
            tabControl.TabPages["tpDataViewer"].Controls.Add(new DataViewer { Dock = DockStyle.Fill });
            tabControl.TabPages["tpTableSplitter"].Controls.Add(new TableSplitter { Dock = DockStyle.Fill });

#if DEBUG
            tabControl.MouseDoubleClick += (sender, e) =>
            {
                // 找到被双击的 Tab 索引
                int tabIndex = -1;
                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    if (tabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tabIndex = i;
                        break;
                    }
                }
                if (tabIndex == -1) return;

                // 控件类型映射
                Func<UserControl>[] creators = new Func<UserControl>[]
                {
                    () => new ImageExtracter { Dock = DockStyle.Fill },
                    () => new DataExtracter { Dock = DockStyle.Fill },
                    () => new DataViewer { Dock = DockStyle.Fill },
                    () => new TableSplitter { Dock = DockStyle.Fill },
                };

                if (tabIndex < creators.Length)
                {
                    var tabPage = tabControl.TabPages[tabIndex];
                    tabPage.Controls.Clear();
                    tabPage.Controls.Add(creators[tabIndex]());
                }
            };
#endif
        }
        #endregion
    }
}
