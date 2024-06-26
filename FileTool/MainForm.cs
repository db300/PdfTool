﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Text = $"文件工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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
                new TabPage("txt文件合并") { BorderStyle = BorderStyle.None, Name = "tpTxtMerger" }
            });
            tabControl.TabPages["tpTxtMerger"].Controls.Add(new TxtMerger { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
