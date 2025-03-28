using PdfSharp.Pdf;
using System;
using System.Data;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PdfDesignHelper
{
    public partial class MainForm : Form
    {
        #region constructor
        public MainForm()
        {
            InitializeComponent();

            InitUi();

            var pfc = new PrivateFontCollection();
            pfc.AddFontFile("HYQiHei_50S.ttf");

            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
            graphics.DrawString("你好，我有一个帽衫", new PdfSharp.Drawing.XFont(pfc.Families[0], 12, PdfSharp.Drawing.XFontStyle.Regular), PdfSharp.Drawing.XBrushes.Black, 10, 10);
            const string filename = "HelloWorld.pdf";
            pdf.Save(filename);
        }
        #endregion

        #region property
        private System.Drawing.Text.PrivateFontCollection _pfc;
        #endregion

        #region ui
        private void InitUi()
        {
            btnOpenTemp.Click += (sender, e) =>
            {
                var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*" };
                if (openDlg.ShowDialog() != DialogResult.OK) return;
                txtPdfTemp.Text = openDlg.FileName;
            };

            numCellCount.ValueChanged += (sender, e) =>
            {
                while (flowLayoutPanel1.Controls.Count > numCellCount.Value) flowLayoutPanel1.Controls.RemoveAt(flowLayoutPanel1.Controls.Count - 1);
                while (flowLayoutPanel1.Controls.Count < numCellCount.Value) flowLayoutPanel1.Controls.Add(new CellControl { Parent = flowLayoutPanel1 });
            };

            btnSelectFont.Click += (sender, e) =>
            {
                var openDlg = new OpenFileDialog { Filter = "PDF文件(*.pdf)|*.pdf|所有文件(*.*)|*.*" };
                if (openDlg.ShowDialog() != DialogResult.OK) return;
                _pfc = new System.Drawing.Text.PrivateFontCollection();
                _pfc.AddFontFile(openDlg.FileName);
            };

            btnPreview.Click += (sender, e) =>
            {
                var document = PdfSharp.Pdf.IO.PdfReader.Open(txtPdfTemp.Text);
                var sb = new StringBuilder();
                var cellList = new CellList();
                foreach (var control in flowLayoutPanel1.Controls)
                    if (control is CellControl cellControl)
                        cellList.Add(cellControl.CellItem);
                foreach (var cellItem in cellList)
                {
                    var page = document.Pages[cellItem.PageNum];
                    using (var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page))
                    {
                        graphics.DrawString(cellItem.Content, new PdfSharp.Drawing.XFont(_pfc.Families[0], 9, PdfSharp.Drawing.XFontStyle.Regular), PdfSharp.Drawing.XBrushes.Black, cellItem.X, cellItem.Y);
                    }

                    page.Close();

                    sb.AppendLine($"{cellItem.PageNum}, {cellItem.Content}, {cellItem.X}, {cellItem.Y}, {cellItem.W}, {cellItem.H}, {cellItem.EmSize}");
                }

                var path = $@"{Application.StartupPath}\tmp\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileName = $"{path}{Guid.NewGuid()}.pdf";
                document.Save(fileName);
                document.Close();
                //iHawkAppLibrary.CmdExecuter.OpenFile(fileName);

                txtLog.Text = sb.ToString();
            };

            btnParse.Click += (sender, e) =>
            {
                var ss = txtLog.Text.Split('\n').ToList().Select(s => s.Trim()).ToList();
                ss.RemoveAll(string.IsNullOrWhiteSpace);
                numCellCount.Value = ss.Count;
                for (var i = 0; i < ss.Count; i++)
                {
                    var s = ss[i];
                    var props = s.Split(',').ToList().Select(a => a.Trim()).ToList();
                    if (flowLayoutPanel1.Controls[i] is CellControl cellControl)
                    {
                        cellControl.CellItem = new CellItem
                        {
                            PageNum = int.Parse(props[0]),
                            Content = props[1],
                            X = float.Parse(props[2]),
                            Y = float.Parse(props[3]),
                            W = float.Parse(props[4]),
                            H = float.Parse(props[5]),
                            EmSize = float.Parse(props[6])
                        };
                    }
                }
            };
        }
        #endregion
    }
}
