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

        private void BtnConvert2Sketch_Click(object sender, EventArgs e)
        {
            foreach(var file in _inputImgFileList)
            {
                ConvertToSketch(file, file + "2.png");
            }
        }
        #endregion

        #region ui
        private void InitUi()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Text = $"图片处理工具 v{Application.ProductVersion}";

            var btnOpen = new Button
            {
                AutoSize = true,
                Location = new System.Drawing.Point(20, 20),
                Parent = this,
                Text = "打开"
            };
            btnOpen.Click += BtnOpen_Click;

            var btnConvert = new Button
            {
                AutoSize = true,
                Location = new System.Drawing.Point(btnOpen.Right + 12, btnOpen.Top),
                Parent = this,
                Text = "转换"
            };
            btnConvert.Click += BtnConvert_Click;

            var btnConvert2Sketch = new Button
            {
                AutoSize = true,
                Location = new System.Drawing.Point(btnConvert.Right + 12, btnOpen.Top),
                Parent = this,
                Text = "转换为素描风格"
            };
            btnConvert2Sketch.Click += BtnConvert2Sketch_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new System.Drawing.Point(20, 50),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new System.Drawing.Size(ClientSize.Width - 20 * 2, ClientSize.Height - 20 - 50),
            };
        }
        #endregion
    }
}
