using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfEditor.Controls
{
    /// <summary>
    /// 页面面板(列表)
    /// </summary>
    public partial class PagePanel : UserControl
    {
        #region constructor
        public PagePanel()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        public int SelectedPageNum { get; private set; } = -1;
        private FlowLayoutPanel _flPanel;
        #endregion

        #region method
        public void AddPage(int pageNum, Image img)
        {
            var panel = new PagePreviewPanel { Dock = DockStyle.Top };
            panel.SetPage(pageNum, img);
            panel.PageSelect += Panel_PageSelect;
            _flPanel.Controls.Add(panel);
        }
        #endregion

        #region event handler
        private void Panel_PageSelect(object sender, EventArgs e, int pageNum)
        {
            if (SelectedPageNum >= 0) _flPanel.Controls[SelectedPageNum].BackColor = SystemColors.Control;
            _flPanel.Controls[pageNum].BackColor = Color.AliceBlue;
            SelectedPageNum = pageNum;
        }
        #endregion

        #region ui
        private void InitUi()
        {
            _flPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                Parent = this
            };
        }
        #endregion
    }
}
