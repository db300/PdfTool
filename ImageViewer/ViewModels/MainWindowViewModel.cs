using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Embedding.Offscreen;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageViewer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    #region constructor
    public MainWindowViewModel()
    {
        ImageGroups = new ObservableCollection<Models.ImageGroupModel>();

        ScanCommand = new RelayCommand(ScanImage);
    }
    #endregion

    #region property
    internal ObservableCollection<Models.ImageGroupModel> ImageGroups { get; }

    private Models.ImageGroupModel? _selectedImageGroup;
    internal Models.ImageGroupModel? SelectedImageGroup
    {
        get => _selectedImageGroup;
        set
        {
            if (_selectedImageGroup != value)
            {
                _selectedImageGroup = value;
                OnSelectedImageGroupChanged();
            }
        }
    }

    private IEnumerable<Models.ImageModel>? _selectedGroupImages;
    internal IEnumerable<Models.ImageModel>? SelectedGroupImages
    {
        get => _selectedGroupImages;
        private set => SetProperty(ref _selectedGroupImages, value);
    }

    public ICommand ScanCommand { get; }
    #endregion

    #region method
    private async void ScanImage()
    {
        if (Avalonia.Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow is not TopLevel topLevel) return;
        var result = await topLevel.StorageProvider.OpenFolderPickerAsync(new Avalonia.Platform.Storage.FolderPickerOpenOptions { Title = "请选择要扫描的文件夹" });
#if DEBUG
        System.Diagnostics.Debug.WriteLine(result);
#endif
        //获取result目录及其所有子目录
        var rootFolder = result.First();
        var rootGroup = new Models.ImageGroupModel(rootFolder.Name, rootFolder.Path.LocalPath, new ObservableCollection<Models.ImageGroupModel>());
        ImageGroups.Clear();
        ImageGroups.Add(rootGroup);
        await ScanDirectoryAsync(rootFolder.Path.LocalPath, rootGroup);
        /*
        //获取result目录及其所有子目录里的图片文件
        var folderPath = result.First().Path.LocalPath;
        var imageFiles = GetImageFiles(folderPath);
        _images = imageFiles.Select(a => new Models.ImageModel(Path.GetFileName(a), Path.GetDirectoryName(a), a));
        */
    }

    private async Task ScanDirectoryAsync(string path, Models.ImageGroupModel parentGroup)
    {
        var directories = Directory.GetDirectories(path);
        foreach (var directory in directories)
        {
            var directoryInfo = new DirectoryInfo(directory);
            var subGroup = new Models.ImageGroupModel(directoryInfo.Name, directory, new ObservableCollection<Models.ImageGroupModel>());
            parentGroup.SubGroups?.Add(subGroup);
            await ScanDirectoryAsync(directory, subGroup);
        }
    }

    private IEnumerable<string> GetImageFiles(string folderPath)
    {
        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        /*
        var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()));
        */
        var files = Directory.GetFiles(folderPath, "*.*")
            .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLower()));
        return files;
    }

    private void OnSelectedImageGroupChanged()
    {
        if (SelectedImageGroup is null) return;
        if (SelectedImageGroup.Images is null)
        {
            var images = GetImageFiles(SelectedImageGroup.Path);
            SelectedImageGroup.Images = new ObservableCollection<Models.ImageModel>(images.Select(a => new Models.ImageModel(Path.GetFileName(a), Path.GetDirectoryName(a), a)));
        }
        SelectedGroupImages = SelectedImageGroup.Images;
    }
    #endregion
}
