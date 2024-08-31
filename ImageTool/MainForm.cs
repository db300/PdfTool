using ImageProcessor;
using ImageProcessor.Imaging.Filters.Photo;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ImageTool
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
        private readonly List<string> _inputImgFileList = new List<string>();
        private TextBox _txtLog;

        private const string Url4Appreciate = "https://www.yuque.com/docs/share/4d2ad434-a4fe-40a1-b530-c61811d5226e?# 《打赏说明》";
        private const string Url4Feedback = "https://www.yuque.com/lengda/eq8cm6/ezwik4?singleDoc# 《需求记录》";
        private const string Url4Readme = "https://www.yuque.com/lengda/eq8cm6/dw961sdll0h3zkz0?singleDoc# 《图片处理工具》";
        #endregion

        #region method
        private void ConvertToSketch(string inputImagePath, string outputImagePath)
        {
            // 读取图像
            var src = Cv2.ImRead(inputImagePath, ImreadModes.Color);
            // 转换为灰度图像
            var gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            // 反转颜色
            var invertedGray = new Mat();
            Cv2.BitwiseNot(gray, invertedGray);
            // 高斯模糊
            var blurred = new Mat();
            Cv2.GaussianBlur(invertedGray, blurred, new OpenCvSharp.Size(21, 21), 0);
            // 反转模糊图像
            var invertedBlurred = new Mat();
            Cv2.BitwiseNot(blurred, invertedBlurred);
            // 创建素描图像
            var sketch = new Mat();
            Cv2.Divide(gray, invertedBlurred, sketch, scale: 256.0);
            // 保存素描图像
            Cv2.ImWrite(outputImagePath, sketch);
        }
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

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "图片文件(*.bmp;*.jpg;*.tif;*.png)|*.bmp;*.jpg;*.tif;*.png|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            _inputImgFileList.Clear();
            _inputImgFileList.AddRange(openDlg.FileNames);
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            foreach (var file in _inputImgFileList)
            {
                var bytes = File.ReadAllBytes(file);
                using (var inputStream = new MemoryStream(bytes))
                {
                    using (var outputStream = new MemoryStream())
                    {
                        using (var imgFactory = new ImageFactory())
                        {
                            imgFactory.Load(inputStream)
                                .Filter(MatrixFilters.BlackWhite)
                                .Save(outputStream);
                        }
                        File.WriteAllBytes(file + "1.png", outputStream.ToArray());
                    }
                }
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"图片处理工具 v{Application.ProductVersion}";

            var lbl = new LinkLabel
            {
                AutoSize = true,
                Location = new System.Drawing.Point(10, 10),
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
            lbl.Location = new System.Drawing.Point(ClientSize.Width - 10 - lbl.Width, 10);

            var lbl2 = new LinkLabel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Parent = this,
                Text = "点击查看使用说明",
            };
            lbl2.LinkClicked += Lbl_LinkClicked2;
            lbl2.Location = new System.Drawing.Point(lbl.Left - 10 - lbl2.Width, 10);

            var tabControl = new TabControl
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new System.Drawing.Point(10, lbl.Bottom + 10),
                Parent = this,
                Size = new System.Drawing.Size(ClientSize.Width - 10 * 2, ClientSize.Height - 10 * 2 - lbl.Bottom)
            };
            tabControl.TabPages.AddRange(new TabPage[]
            {
                new TabPage("素描化") { BorderStyle = BorderStyle.None, Name = "tpSketchConverter" }
            });
            tabControl.TabPages["tpSketchConverter"].Controls.Add(new SketchConverter { Dock = DockStyle.Fill });

            var btnOpen = new Button
            {
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20),
                Parent = this,
                Text = "打开",
                Visible = false
            };
            btnOpen.Click += BtnOpen_Click;

            var btnConvert = new Button
            {
                AutoSize = true,
                Location = new System.Drawing.Point(btnOpen.Right + 12, btnOpen.Top),
                Parent = this,
                Text = "转换",
                Visible = false
            };
            btnConvert.Click += BtnConvert_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new System.Drawing.Point(20, 50),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new System.Drawing.Size(ClientSize.Width - 20 * 2, ClientSize.Height - 20 - 50),
                Visible = false
            };
        }
        #endregion
    }
}
