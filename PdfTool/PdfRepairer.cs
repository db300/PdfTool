using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfTool
{
    /// <summary>
    /// PDF修复器
    /// </summary>
    public partial class PdfRepairer : UserControl
    {
        #region constructor
        public PdfRepairer()
        {
            InitializeComponent();

            InitUi();
        }
        #endregion

        #region property
        private TextBox _txtLog;
        #endregion

        #region method
        private int TryOpenPdf(string fileName)
        {
            try
            {
                var pageCount = PdfHelperLibrary.CommonHelper.GetPageCount(fileName);
                _txtLog.AppendText($"【页数：{pageCount}】{fileName}\r\n");
                return pageCount;
            }
            catch (Exception ex)
            {
                _txtLog.AppendText($"{fileName} 加载失败: {ex.Message}\r\n");
                return -1;
            }
        }
        #endregion

        #region event handler
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*", Multiselect = true };
            if (openDlg.ShowDialog() != DialogResult.OK) return;
            var inputFilenames = openDlg.FileNames;
            foreach (var inputFilename in inputFilenames)
            {
                _txtLog.AppendText("---------------------------------------------\r\n");
                if (TryOpenPdf(inputFilename) >= 0) continue;

                var repairMsg = PdfHelperLibrary2.RepairHelper.SaveAs(inputFilename, out var outputFilename);
                if (!string.IsNullOrWhiteSpace(repairMsg))
                {
                    _txtLog.AppendText($"{inputFilename} 修复失败: {repairMsg}\r\n");
                    continue;
                }

                if (TryOpenPdf(outputFilename) >= 0)
                {
                    _txtLog.AppendText($"{inputFilename} 修复完成: {outputFilename}\r\n");
                }
                else
                {
                    _txtLog.AppendText($"{inputFilename} 修复失败: 修复后仍无法顺利加载\r\n");
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
                Location = new Point(Config.ControlMargin, Config.ControlMargin),
                Parent = this,
                Text = "打开文件并修复"
            };
            btnOpen.Click += BtnOpen_Click;

            var top = btnOpen.Bottom + Config.ControlPadding;
            _txtLog = new TextBox
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font(Font.FontFamily, 11f, GraphicsUnit.Point),
                Location = new Point(Config.ControlMargin, top),
                Multiline = true,
                Parent = this,
                ReadOnly = true,
                ScrollBars = ScrollBars.Both,
                Size = new Size(ClientSize.Width - Config.ControlMargin * 2, ClientSize.Height - Config.ControlMargin - top),
                WordWrap = false
            };
        }
        #endregion
    }
}
