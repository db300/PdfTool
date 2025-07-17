using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfEditor.Modules
{
    /// <summary>
    /// 页面排序器
    /// </summary>
    public partial class PageSorter : PageOperaBase
    {
        #region constructor
        public PageSorter()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        #endregion

        #region event handler
        protected override void PagePanel_PageSelect(object sender, EventArgs e, int pageNum)
        {
            base.PagePanel_PageSelect(sender, e, pageNum);
        }
        #endregion

        #region ui
        private void InitUi()
        {

        }
        #endregion
    }
}
