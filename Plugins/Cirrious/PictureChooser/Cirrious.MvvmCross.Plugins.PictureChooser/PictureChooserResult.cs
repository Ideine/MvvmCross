using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrious.MvvmCross.Plugins.PictureChooser
{
    public class PictureChooserResult
    {
        public enum PictureChooserResultState
        {
            OK,
            Canceled,
            InvalidImage
        }

        public Stream PictureSream { get; set; }
        public PictureChooserResultState ResultState { get; set; }
        public string OriginalFilePath { get; set; }
    }
}
