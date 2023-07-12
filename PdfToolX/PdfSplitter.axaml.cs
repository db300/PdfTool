using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PdfSharpCore.Pdf.IO;
using System.Collections.Generic;
using System.Linq;

namespace PdfToolX
{
    public partial class PdfSplitter : UserControl
    {
        #region constructor
        public PdfSplitter()
        {
            InitializeComponent();
        }
        #endregion

        #region property
        private readonly List<string> _inputPdfFileList = new List<string>();
        #endregion

        #region event handler
        private async void BtnAddFile_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new OpenFileDialog
            {
                AllowMultiple = true,
                Filters =
                {
                    new FileDialogFilter { Name = "pdf文件", Extensions = new List<string> { "pdf" } },
                    new FileDialogFilter { Name = "所有文件", Extensions = new List<string> { "*" } }
                }
            };
            if (this.GetVisualRoot() is not Window mainWindow) return;
            var result = await openDlg.ShowAsync(mainWindow);
            if (!(result?.ToList().Count > 0)) return;
            _inputPdfFileList.Clear();
            _inputPdfFileList.AddRange(result.ToList());
            foreach (var file in _inputPdfFileList)
            {
                var document = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                _txtLog.Text += $"【页数：{document.PageCount}】{file}\r\n";
            }
        }

        private void BtnSplit_Click(object sender, RoutedEventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要拆分的PDF文件\r\n";
                return;
            }
            foreach (var fileName in _inputPdfFileList)
            {
                var s = PdfHelperLibraryX.SplitHelper.SplitPdf(fileName, (int)_numPagePerDoc.Value);
                if (string.IsNullOrWhiteSpace(s)) _txtLog.Text += $"{fileName} 拆分完成\r\n";
                else _txtLog.Text += $"{fileName} {s}\r\n";
            }
            _txtLog.Text += "拆分完成\r\n";
        }

        private void BtnExtract_Click(object sender, RoutedEventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "未添加需要提取的PDF文件\r\n";
                return;
            }
            if (_inputPdfFileList.Count != 1)
            {
                _txtLog.Text = "添加了多个PDF文件，只对第一个文件进行提取\r\n";
            }
            var s = PdfHelperLibraryX.ExtractHelper.ExtractPdf(_inputPdfFileList[0], (int)_numPageFrom.Value, (int)_numPageTo.Value, out var outputPdfFile);
            if (string.IsNullOrWhiteSpace(s)) _txtLog.Text += $"提取完成: {outputPdfFile}\r\n";
            else _txtLog.Text += $"{s}\r\n";
        }
        #endregion
    }
}
