using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PdfSharpCore.Pdf.IO;
using System.Collections.Generic;
using System.Linq;

namespace PdfToolX
{
    public partial class PdfMerger : UserControl
    {
        #region constructor
        public PdfMerger()
        {
            InitializeComponent();
        }
        #endregion

        #region event handler
        private async void BtnAddFile_Click(object sender, RoutedEventArgs e)
        {
            var openDlg = new OpenFileDialog
            {
                AllowMultiple = true,
                Filters =
                {
                    new FileDialogFilter { Name = "pdf�ļ�", Extensions = new List<string> { "pdf" } },
                    new FileDialogFilter { Name = "�����ļ�", Extensions = new List<string> { "*" } }
                }
            };
            if (this.GetVisualRoot() is not Window mainWindow) return;
            var result = await openDlg.ShowAsync(mainWindow);
            if (!(result?.ToList().Count > 0)) return;
            var inputFileList = result.ToList();
            foreach (var file in inputFileList)
            {
                var document = PdfReader.Open(file, PdfDocumentOpenMode.Import);
                _txtLog.Text += $"��ҳ����{document.PageCount}��{file}\r\n";
                _txtFileList.Text += $"{file}\r\n";
            }
        }

        private void BtnMerge_Click(object sender, RoutedEventArgs e)
        {
            var inputPdfFilenameList = _txtFileList.Text.Split('\n').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            if (inputPdfFilenameList.Count == 0)
            {
                _txtLog.Text += "δ�����Ҫ�ϲ���PDF�ļ�\r\n";
                return;
            }
            var s = PdfHelperLibraryX.MergeHelper.MergePdf(inputPdfFilenameList, out var outputPdfFilename);
            if (string.IsNullOrWhiteSpace(s)) _txtLog.Text += $"�ϲ����: {outputPdfFilename}\r\n";
            else _txtLog.Text += $"{s}\r\n";
        }
        #endregion
    }
}
