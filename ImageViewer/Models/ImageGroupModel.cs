using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Models
{
    internal class ImageGroupModel
    {
        #region constructor
        public ImageGroupModel(string title, string path, ObservableCollection<ImageGroupModel> subGroups)
        {
            Title = title;
            Path = path;
            SubGroups = subGroups;
        }
        #endregion

        #region property
        public string? Title { get; }
        public string? Path { get; }
        public ObservableCollection<ImageGroupModel>? SubGroups { get; }
        public ObservableCollection<ImageModel>? Images { get; set; }
        #endregion
    }
}
