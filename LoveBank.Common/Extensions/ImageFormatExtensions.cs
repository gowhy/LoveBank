using System.Collections.Generic;
using System.Drawing.Imaging;

namespace LoveBank.Common.Extensions
{
    public class ImageFormatAndStuffix {
        private static readonly Dictionary<string, ImageFormat> stuffixDictionary;

        static ImageFormatAndStuffix() {
            stuffixDictionary = new Dictionary<string, ImageFormat>();
            stuffixDictionary.Add("png", ImageFormat.Png);
            stuffixDictionary.Add("jpg", ImageFormat.Jpeg);
            stuffixDictionary.Add("gif", ImageFormat.Gif);
        }

        public ImageFormat this[string stuffix] {
            get { return stuffix.Contains(stuffix) ? stuffixDictionary[stuffix] : ImageFormat.Jpeg; }
        }
    }

    public static class ImageFormatExtensions {
        public static ImageFormat ToImageFormat(this string stuffix) {
            stuffix = stuffix.ToLower();
            return new ImageFormatAndStuffix()[stuffix.ToLower()];
        }
    }
}
