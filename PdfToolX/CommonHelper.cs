using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PdfToolX
{
    internal static class CommonHelper
    {
        internal static async Task<IReadOnlyList<IStorageFile>?> OpenPdfFileAsync(Visual visual)
        {
            var topLevel = TopLevel.GetTopLevel(visual);
            if (topLevel is null) return null;
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                AllowMultiple = true,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new("pdf文件") { Patterns = new List<string> { "*.pdf" } },
                    new("所有文件") { Patterns = new List<string> { "*.*" } }
                }
            });
            return files;
        }
    }
}
