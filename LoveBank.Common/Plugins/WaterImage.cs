using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LoveBank.Common.Plugins
{
    public static class WaterImage
    {

        public static Image AddWaterStringToImage(Image image, string addText, string fontFamily, int fontSize, Color color, float x, float y)
        {
            var graphics = Graphics.FromImage(image);
            graphics.DrawImage(image, 0, 0);
            var font = new Font(fontFamily, fontSize);
            Brush brush = new SolidBrush(color);
            graphics.DrawString(addText, font, brush, x, y);
            graphics.Dispose();
            return image;
        }

    }
}
