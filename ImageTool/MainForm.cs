using ImageProcessor;
using ImageProcessor.Imaging.Filters.Photo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #endregion

        #region ui
        private void InitUi()
        {
            var btnOpen = new Button
            {
                AutoSize = true,
                Location = new Point(20, 20),
                Parent = this,
                Text = "打开"
            };
            btnOpen.Click += BtnOpen_Click;

            var btnConvert = new Button
            {
                AutoSize = true,
                Location = new Point(btnOpen.Right + 12, btnOpen.Top),
                Parent = this,
                Text = "转换"
            };
            btnConvert.Click += BtnConvert_Click;

            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Location = new Point(20, 50),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - 20 * 2, ClientSize.Height - 20 - 50),
            };
        }
        #endregion
    }
}
