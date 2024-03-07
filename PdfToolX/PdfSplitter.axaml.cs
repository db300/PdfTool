using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using System.Collections.Generic;
using System.Linq;

namespace PdfToolX;

/// <summary>
/// PDF拆分器
/// </summary>
public partial class PdfSplitter : UserControl
{
    #region constructor
    public PdfSplitter()
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
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is null) return;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = true,
            FileTypeFilter = new List<FilePickerFileType>
            {
                new("pdf文件") { Patterns = new List<string> { "*.pdf" } },
                new("所有文件") { Patterns = new List<string> { "*.*" } }
            }
        });
        if (!(files?.Count > 0)) return;
        _inputPdfFileList.Clear();
        _inputPdfFileList.AddRange(files.Select(a => a.Path.LocalPath));
        _inputPdfFileList.Select(a => $"【页数：{PdfHelperLibraryX.CommonHelper.GetPageCount(a)}】{a}").ToList().ForEach(a => _txtLog.Text += $"{a}\r\n");
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