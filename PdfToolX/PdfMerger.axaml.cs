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
/// PDF合并器
/// </summary>
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
        var inputFileList = files.Select(a => a.Path.LocalPath).ToList();
        foreach (var file in inputFileList)
        {
            _txtLog.Text += $"【页数：{PdfHelperLibraryX.CommonHelper.GetPageCount(file)}】{file}\r\n";
            _txtFileList.Text += $"{file}\r\n";
        }
    }

    private void BtnMerge_Click(object sender, RoutedEventArgs e)
    {
        var inputPdfFilenameList = _txtFileList?.Text?.Split('\n').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        if (!(inputPdfFilenameList?.Count > 0))
        {
            _txtLog.Text += "未添加需要合并的PDF文件\r\n";
            return;
        }
        var s = PdfHelperLibraryX.MergeHelper.MergePdf(inputPdfFilenameList, out var outputPdfFilename);
        if (string.IsNullOrWhiteSpace(s)) _txtLog.Text += $"合并完成: {outputPdfFilename}\r\n";
        else _txtLog.Text += $"{s}\r\n";
    }
    #endregion
}