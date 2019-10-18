using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Modifiers;
using System.Drawing;
using System.Drawing.Imaging;

namespace MovementDetection
{
    public class Heatmap
    {
        private int width, height;
        private Modifier decay, growth, dropoff;
        private int radius, radiusSqr;
        private float interval;

        private (int x, int y, float dist)[] affectedPixels;

        // CHANGE BACK TO PRIVATE
        public float[,] map;

        public Heatmap(int width, int height, Modifier decay, Modifier growth, Modifier dropoff, int radius, float interval)
        {
            this.width = width;
            this.height = height;

            this.radius = radius;
            radiusSqr = radius * radius;

            this.interval = interval;

            this.decay = decay;
            this.growth = growth;
            this.dropoff = dropoff;

            map = new float[width, height];

            UpdateAffectedPixels();
        }
        
        public void Plot(int x, int y, float strength)
        {
            foreach ((int offsetX, int offsetY, float distance) in affectedPixels)
            {
                int currentX = x + offsetX;
                int currentY = y + offsetY;

                // Check if the new coordinates are in bounds
                if (currentX < 0 || currentY < 0)
                    continue;
                if (currentX >= width || currentY >= height)
                    continue;

                float dropoffStrength = dropoff.Apply(1f - distance / radius, 1f);

                map[currentX, currentY] = growth.Apply(map[currentX, currentY], strength * dropoffStrength * interval);
            }
        }

        public void Step()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = decay.Apply(map[x, y], interval);
                }
            }
        }

        public Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int brightness = (int)Math.Round(255 * map[x, y]);
                    bitmap.SetPixel(x, y, Color.FromArgb(brightness, brightness, brightness));
                }
            }

            return bitmap;
        }

        private void UpdateAffectedPixels()
        {
            List<(int x, int y, float dist)> affectedPixels = new List<(int x, int y, float dist)>();

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    float distance = (float)Math.Sqrt(x * x + y * y);

                    if (distance <= radius)
                    {
                        affectedPixels.Add((x, y, distance));
                    }
                }
            }

            this.affectedPixels = affectedPixels.ToArray();
        }
    }
}
