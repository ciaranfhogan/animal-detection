using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Extensions;
using System.Drawing.Imaging;

namespace MovementDetection.Pipeline
{
    /// <summary>
    /// Allows changes to be made to a frame without being immediately applied. This is useful
    /// if the previous frames neighbouring values determine the new frames values.
    /// </summary>
    public class BufferedFrame
    {
        public readonly int width, height;

        /// <summary>
        /// Specifies how out of bounds coordinates are handled:
        /// 1) Extend: extend the edge pixels.
        /// 2) Wrap: wrap around the the other side of the frame.
        /// 3) Ignore: do nothing.
        /// </summary>
        public enum OutOfBoundsSetMode { Extend, Wrap, Ignore};
        /// <summary>
        /// Specifies how out of bounds coordinates are handled:
        /// 1) Extend: extend the edge pixels.
        /// 2) Wrap: wrap around the the other side of the frame.
        /// </summary>
        public enum OutOfBoundsGetMode { Extend, Wrap};

        private float[,] frame, buffer;

        public BufferedFrame(int width, int height)
        {
            this.width = width;
            this.height = height;

            frame = new float[width, height];
            buffer = new float[width, height];
        }

        public BufferedFrame(float[,] frame)
        {
            width = frame.GetLength(0);
            height = frame.GetLength(1);

            this.frame = frame;
            buffer = (float[,])frame.Clone();
        }

        public void SetBufferValue(int x, int y, float value)
        {
            buffer[x, y] = value;
        }

        public void SetBufferValue(int x, int y, float value, OutOfBoundsSetMode mode)
        {
            if (IsOutOfBounds(x, y))
            {
                switch (mode)
                {
                    case OutOfBoundsSetMode.Extend:
                        x = MathExtension.Clamp(x, 0, width);
                        y = MathExtension.Clamp(y, 0, height);
                        break;
                    case OutOfBoundsSetMode.Wrap:
                        x = MathExtension.Wrap(x, 0, width);
                        y = MathExtension.Wrap(y, 0, height);
                        break;
                    case OutOfBoundsSetMode.Ignore:
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            buffer[x, y] = value;
        }

        /// <summary>
        /// Returns the value of the pixel at the specified position. Note: Apply()
        /// must be called to see any changes made with the SetPixel() or
        /// SetPixelNoCheck() methods.
        /// </summary>
        public float GetValue(int x, int y)
        {
            return frame[x, y];
        }

        public float GetValue(int x, int y, OutOfBoundsGetMode mode)
        {
            if (IsOutOfBounds(x, y))
            {
                switch (mode)
                {
                    case OutOfBoundsGetMode.Extend:
                        x = MathExtension.Clamp(x, 0, width);
                        y = MathExtension.Clamp(y, 0, height);
                        break;
                    case OutOfBoundsGetMode.Wrap:
                        x = MathExtension.Wrap(x, 0, width);
                        y = MathExtension.Wrap(y, 0, height);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            return frame[x, y];
        }

        public float GetBufferValue(int x, int y)
        {
            return buffer[x, y];
        }

        public float GetBufferValue(int x, int y, OutOfBoundsGetMode mode)
        {
            if (IsOutOfBounds(x, y))
            {
                switch (mode)
                {
                    case OutOfBoundsGetMode.Extend:
                        x = MathExtension.Clamp(x, 0, width);
                        y = MathExtension.Clamp(y, 0, height);
                        break;
                    case OutOfBoundsGetMode.Wrap:
                        x = MathExtension.Wrap(x, 0, width);
                        y = MathExtension.Wrap(y, 0, height);
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            return buffer[x, y];
        }

        public bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || y < 0 || x >= width || y >= height;
        }

        /// <summary>
        /// Apply the changes made to the buffer to the frame.
        /// </summary>
        public void Apply()
        {
            Array.Copy(buffer, frame, width * height);
        }

        /// <summary>
        /// Resets the buffer with values from the frame.
        /// </summary>
        public void Reset()
        {
            Array.Copy(frame, buffer, width * height);
        }

        public void Fill(float[,] array)
        {
            if (array.GetLength(0) != width || array.GetLength(1) != height)
                throw new ArgumentOutOfRangeException("The width and height of the array must" +
                    " be the same as the BufferedFrame's width and height.");

            Array.Copy(array, frame, width * height);
            Array.Copy(array, buffer, width * height);
        }

        /// <summary>
        /// Writes the contents of the frame to the specified array.
        /// </summary>
        public void WriteToArray(float[,] array)
        {
            Array.Copy(frame, array, width * height);
        }

        /// <summary>
        /// Save the frame as a PNG.
        /// </summary>
        public void SaveFrame(string directory, string prefix, int iteration)
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int c = (int)Math.Round(255 * frame[x, y]);
                    bitmap.SetPixel(x, y, Color.FromArgb(c, c, c));
                }
            }

            bitmap.Save(Path.Combine(directory, prefix + iteration + ".png"), ImageFormat.Png);
        }
    }
}
