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
                    new FileDialogFilter { Name = "pdf�ļ�", Extensions = new List<string> { "pdf" } },
                    new FileDialogFilter { Name = "�����ļ�", Extensions = new List<string> { "*" } }
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
                _txtLog.Text += $"��ҳ����{document.PageCount}��{file}\r\n";
            }
        }

        private void BtnSplit_Click(object sender, RoutedEventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "δ�����Ҫ��ֵ�PDF�ļ�";
                return;
            }
            foreach (var fileName in _inputPdfFileList)
            {

            }
            _txtLog.Text += "������\r\n";
        }

        private void BtnExtract_Click(object sender, RoutedEventArgs e)
        {
            if (_inputPdfFileList.Count == 0)
            {
                _txtLog.Text = "δ�����Ҫ��ȡ��PDF�ļ�";
                return;
            }
            if (_inputPdfFileList.Count != 1)
            {
                _txtLog.Text = "����˶��PDF�ļ���ֻ�Ե�һ���ļ�������ȡ";
            }
        }
        #endregion
    }
}
