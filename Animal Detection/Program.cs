using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Video.FFMPEG;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using MovementDetection.Extensions;

namespace MovementDetection
{
    public class Program
    {
        private static string videoPath = @"D:\Projects\Animal Detection\move.mp4";

        private static int imageWidth = 360;

        private static float startTime = 0f;
        private static float endTime = 2f;

        public static void Main(string[] args)
        {
            VideoFileReader reader = new VideoFileReader();
            reader.Open(videoPath);

            float frameRate = (float)reader.FrameRate.ToDouble();

            int startFrame = MathExtension.Clamp((int)Math.Floor(startTime * frameRate), 0, (int)reader.FrameCount);
            int endFrame = MathExtension.Clamp((int)Math.Floor(endTime * frameRate), 0, (int)reader.FrameCount);

            int frameCount = endFrame - startFrame;
            Size size = new Size(imageWidth, imageWidth * reader.Height / reader.Width);
            float[][,] frames = new float[frameCount][,];

            Console.Write("Reading frames".PadRight(30));
            for (int videoIndex = startFrame, frameIndex = 0; videoIndex < endFrame; videoIndex++, frameIndex++)
            {
                using (Bitmap bitmap = new Bitmap(reader.ReadVideoFrame(videoIndex), size))
                {
                    frames[frameIndex] = bitmap.ConvertToGrayscaleImage();
                }

                if (frameIndex % ((int)Math.Floor(frameCount / 10f)) == 0)
                    Console.Write(".");
            }
            Console.Write(" Done\n");

            MotionDetector detector = new MotionDetector(frames, frameRate);
            detector.Process();

            Console.Write("Closing in 10 seconds...");
            System.Threading.Thread.Sleep(10000);
        }

        private static void WriteLine(string message, ConsoleColor color)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = consoleColor;
        }

        private static string ReadLine(string prompt, ConsoleColor color)
        {
            ConsoleColor consoleColor = Console.ForegroundColor;

            Console.Write(prompt);
            Console.ForegroundColor = color;
            string input = Console.ReadLine();
            Console.ForegroundColor = consoleColor;

            return input;
        }
    }
}
