﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
        #endregion

        #region property
        private const string Url4Appreciate = "https://www.yuque.com/lengda/eq8cm6/rylia4";
        private const string Url4Feedback = "https://www.yuque.com/lengda/eq8cm6/ezwik4";
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/zqfgwqx6g6azmkho";
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

        private void Lbl_LinkClicked2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Url4Readme);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"Excel工具 {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";

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

            var lbl2 = new LinkLabel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Parent = this,
                Text = "点击查看使用说明",
            };
            lbl2.LinkClicked += Lbl_LinkClicked2;
            lbl2.Location = new Point(lbl.Left - 10 - lbl2.Width, 10);

            var tabControl = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(10, lbl.Bottom + 10),
                Parent = this,
                Size = new Size(ClientSize.Width - 10 * 2, ClientSize.Height - 10 * 2 - lbl.Bottom)
            };
            tabControl.TabPages.AddRange(new TabPage[]
            {
                new TabPage("图片提取") { BorderStyle = BorderStyle.None, Name = "tpImageExtracter" }
            });
            tabControl.TabPages["tpImageExtracter"].Controls.Add(new ImageExtracter { Dock = DockStyle.Fill });
        }
        #endregion
    }
}
