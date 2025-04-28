using System;
using System.Drawing;
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

        public void UpdatePage(int pageNum, Image img)
        {
            if (pageNum < 0 || pageNum >= _flPanel.Controls.Count || !(_flPanel.Controls[pageNum] is PagePreviewPanel panel)) return;
            panel.SetPage(pageNum, img);
        }
        #endregion

        #region event handler
        private void Panel_PageSelect(object sender, EventArgs e, int pageNum)
        {
            if (SelectedPageNum >= 0) _flPanel.Controls[SelectedPageNum].BackColor = SystemColors.Control;
            _flPanel.Controls[pageNum].BackColor = /*Color.AliceBlue*/Color.FromArgb(249, 244, 255);
            SelectedPageNum = pageNum;

            PageSelect?.Invoke(sender, e, pageNum);
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

        #region custom event
        public delegate void PageSelectHandler(object sender, EventArgs e, int pageNum);
        public event PageSelectHandler PageSelect;
        #endregion
    }
}
