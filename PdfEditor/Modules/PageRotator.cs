using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfEditor.Modules
{
    /// <summary>
    /// 页面旋转器
    /// </summary>
    public partial class PageRotator : UserControl
    {
        #region constructor
        public PageRotator()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private FlowLayoutPanel _flPanel;
        private PictureBox _picBox;
        #endregion

        #region ui
        private void InitUi()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Right,
                Parent = this,
                Width = 200
            };
            var btnRotateLeft = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = panel,
                Text = "逆时针旋转"
            };
            var btnRotateRight = new Button
            {
                AutoSize = true,
                Location = new Point(Config.ControlMargin, btnRotateLeft.Bottom + Config.ControlPadding),
                Parent = panel,
                Text = "顺时针旋转"
            };

            _flPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Parent = this,
                Width = 200
            };

            _picBox = new PictureBox
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Parent = this
            };
            _picBox.BringToFront();
        }
        #endregion
    }
}
