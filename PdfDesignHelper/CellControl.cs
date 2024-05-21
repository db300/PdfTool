using System.Windows.Forms;

namespace PdfDesignHelper
{
    public partial class CellControl : UserControl
    {
        #region constructor
        public CellControl()
        {
            InitializeComponent();
        }
        #endregion

        #region property
        internal CellItem CellItem
        {
            get => new CellItem
            {
                PageNum = (int)numPageNum.Value,
                Content = txtContent.Text,
                X = (float)numX.Value,
                Y = (float)numY.Value,
                W = (float)numW.Value,
                H = (float)numH.Value,
                EmSize = (float)numEmSize.Value
            };
            set
            {
                numPageNum.Value = value.PageNum;
                txtContent.Text = value.Content;
                numX.Value = (decimal)value.X;
                numY.Value = (decimal)value.Y;
                numW.Value = (decimal)value.W;
                numH.Value = (decimal)value.H;
                numEmSize.Value = (decimal)value.EmSize;
            }
        }
        #endregion
    }
}
