using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MovementDetection.Extensions
{
    public static class BitmapExtension
    {
        public static float[,] ConvertToGrayscaleImage(this Bitmap bitmap)
        {
            float[,] image = new float[bitmap.Width, bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    image[x, y] = bitmap.GetPixel(x, y).GetBrightness();
                }
            }

            return image;
        }
    }
}
