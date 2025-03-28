using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Models
{
    internal class ImageModel
    {
        #region constructor
        public ImageModel(string title, string path, string filename)
        {
            Title = title;
            Path = path;
            FileName = filename.Replace('\\', '/');
        }
        #endregion

        #region property
        public string? Title { get; }
        public string? Path { get; }
        public string? FileName { get; }
        public Bitmap? Image
        {
            get
            {
                if (FileName == null) return null;
                try
                {
                    return new Bitmap(FileName);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        #endregion
    }
}
