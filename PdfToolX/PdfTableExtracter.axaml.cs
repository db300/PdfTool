using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PdfToolX;

/// <summary>
/// PDF表格提取器
/// </summary>
public partial class PdfTableExtracter : UserControl
{
    #region constructor
    public PdfTableExtracter()
    {
        InitializeComponent();
    }
    #endregion

    #region property
    private readonly List<string> _inputPdfFileList = new();
    #endregion

    #region event handler
    private async void BtnAddFile_Click(object sender, RoutedEventArgs e)
    {
        var files = await CommonHelper.OpenPdfFileAsync(this);
        if (!(files?.Count > 0)) return;
        _inputPdfFileList.Clear();
        _inputPdfFileList.AddRange(files.Select(a => a.Path.LocalPath));
        _inputPdfFileList.Select(a => $"【页数：{PdfHelperLibrary.CommonHelper.GetPageCount(a)}】{a}").ToList().ForEach(a => _txtLog.Text += $"{a}\r\n");
    }

    private void BtnExtract_Click(object sender, RoutedEventArgs e)
    {
        if (_inputPdfFileList.Count == 0)
        {
            _txtLog.Text = "未添加需要提取的PDF文件\r\n";
            return;
        }
        var background = new BackgroundWorker { WorkerReportsProgress = true };
        background.DoWork += (ww, ee) =>
        {
            foreach (var fileName in _inputPdfFileList)
            {
                var (success, s, pdfExtractTables) = PdfHelperLibrary.TableExtractHelper.ExtractTable(fileName);
                var msg = success ? $"{fileName} 提取完成" : $"{fileName} {s}";
                background.ReportProgress(0, msg);
            }
        };
        background.ProgressChanged += (ww, ee) => _txtLog.Text += $"{ee.UserState}\r\n";
        background.RunWorkerCompleted += (ww, ee) => _txtLog.Text += "提取完成\r\n";
        background.RunWorkerAsync();
        _txtLog.Text += "正在提取，请稍候...\r\n";
    }
    #endregion
}