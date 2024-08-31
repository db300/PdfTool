using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageTool
{
    /// <summary>
    /// 素描转换器
    /// </summary>
    public partial class SketchConverter : UserControl
    {
        #region constructor
        public SketchConverter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private string _inputImgFile;
        private PictureBox _picSrc;
        private PictureBox _picDes;
        private TrackBar _trackBarBlurKernelSize;
        private TrackBar _trackBarBlurSigma;
        private TrackBar _trackBarSketchScale;
        #endregion

        #region method
        #endregion

        #region event handler
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "图片文件(*.bmp;*.jpg;*.tif;*.png)|*.bmp;*.jpg;*.tif;*.png|所有文件(*.*)|*.*" };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            _inputImgFile = openDlg.FileName;
            _picSrc.Image = Image.FromFile(_inputImgFile);
            _picDes.Image = Helpers.SketchHelper.ConvertToSketch(_inputImgFile, _trackBarBlurKernelSize.Value, _trackBarBlurSigma.Value, _trackBarSketchScale.Value);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_inputImgFile)) return;
            _trackBarBlurKernelSize.Value = 21;
            _trackBarBlurSigma.Value = 0;
            _trackBarSketchScale.Value = 256;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_inputImgFile)) return;
            var saveDlg = new SaveFileDialog { DefaultExt = ".png", Filter = "PNG文件(*.png)|*.png|JPG文件(*.jpg)|*.jpg|BMP文件(*.bmp)|*.bmp|所有文件(*.*)|*.*" };
            if (saveDlg.ShowDialog() != DialogResult.OK) return;
            Helpers.SketchHelper.ConvertToSketch(_inputImgFile, saveDlg.FileName, _trackBarBlurKernelSize.Value, _trackBarBlurSigma.Value, _trackBarSketchScale.Value);
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_inputImgFile)) return;
            _picDes.Image = Helpers.SketchHelper.ConvertToSketch(_inputImgFile, _trackBarBlurKernelSize.Value, _trackBarBlurSigma.Value, _trackBarSketchScale.Value);
        }
        #endregion

        #region ui
        private void InitUi()
        {
            var panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                Parent = this
            };
            var panelBody = new Panel
            {
                Dock = DockStyle.Fill,
                Parent = this
            };
            panelBody.BringToFront();

            InitUi4Header(panelHeader);
            InitUi4Body(panelBody);
        }

        private void InitUi4Header(Panel panel)
        {
            var btnOpen = new Button
            {
                AutoSize = true,
                Location = new Point(20, 20),
                Parent = panel,
                Text = "打开"
            };
            btnOpen.Click += BtnOpen_Click;

            var btnReset= new Button
            {
                AutoSize = true,
                Location = new Point(btnOpen.Right + 12, 20),
                Parent = panel,
                Text = "重置"
            };
            btnReset.Click += BtnReset_Click;

            var btnSave = new Button
            {
                AutoSize = true,
                Location = new Point(btnReset.Right + 12, 20),
                Parent = panel,
                Text = "保存"
            };
            btnSave.Click += BtnSave_Click;

            _trackBarBlurKernelSize = new TrackBar
            {
                Location = new Point(20, 60),
                Maximum = 50,
                Minimum = 1,
                Parent = panel,
                TickFrequency = 2,
                Value = 21,
                Width = 200
            };
            _trackBarBlurKernelSize.ValueChanged += TrackBar_ValueChanged;

            _trackBarBlurSigma = new TrackBar
            {
                Location = new Point(240, 60),
                Maximum = 100,
                Minimum = 0,
                Parent = panel,
                TickFrequency = 5,
                Value = 0,
                Width = 200
            };
            _trackBarBlurSigma.ValueChanged += TrackBar_ValueChanged;

            _trackBarSketchScale = new TrackBar
            {
                Location = new Point(460, 60),
                Maximum = 500,
                Minimum = 1,
                Parent = panel,
                TickFrequency = 10,
                Value = 256,
                Width = 200
            };
            _trackBarSketchScale.ValueChanged += TrackBar_ValueChanged;
        }

        private void InitUi4Body(Panel panel)
        {
            _picSrc = new PictureBox
            {
                BackColor = Color.White,
                Parent = panel,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            _picDes = new PictureBox
            {
                BackColor = Color.White,
                Parent = panel,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            panel.ClientSizeChanged += (sender, e) =>
            {
                //调整_picSrc尺寸及位置使其占据panel的左半部分
                _picSrc.Location = new Point(20, 20);
                _picSrc.Height = panel.ClientSize.Height - 40;
                _picSrc.Width = panel.ClientSize.Width / 2 - 20;
                //调整_picDes尺寸及位置使其占据panel的右半部分
                _picDes.Location = new Point(panel.ClientSize.Width / 2, 20);
                _picDes.Height = panel.ClientSize.Height - 40;
                _picDes.Width = panel.ClientSize.Width / 2 - 20;
            };
        }
        #endregion
    }
}
