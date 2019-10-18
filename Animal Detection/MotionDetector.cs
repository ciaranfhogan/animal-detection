using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MovementDetection.Modifiers;
using MovementDetection.Filters;
using MovementDetection.Pipeline;


namespace MovementDetection
{
    public class MotionDetector
    {
        private readonly int width, height;
        private readonly float frameRate;
        private float[][,] frames;

        private readonly Heatmap heatmap;

        private readonly Filter[] pipeline;

        public MotionDetector(float[][,] frames, float frameRate)
        {
            this.frames = frames;
            this.frameRate = frameRate;

            width = frames[0].GetLength(0);
            height = frames[0].GetLength(1);

            var decay = new Modifiers.Decay.Linear(1 / 100f);
            var growth = new Modifiers.Growth.Linear(1 / 50f);
            var dropoff = new Modifiers.Dropoff.Linear();

            heatmap = new Heatmap(width, height, decay, growth, dropoff, 6, 1f / frameRate);

            pipeline = new Filter[]
            {
                //new BoxBlurFilter(1, 1),
                new DenoiseFilter(2, 10, 2),
                new HysteresisFilter(0.01f, 0.02f)
                //new DifferenceFilter(),
                //new HeatmapFilter(heatmap),
                //new MultiplyFilter(heatmap.map)
            };
        }

        public void ExecutePipeline(BufferedFrame bufferedFrame)
        {
            Console.WriteLine("Executing pipeline...");

            for (int i = 0; i < pipeline.Length; i++)
            {
                Console.WriteLine("\tStep " + i + ": " + pipeline[i].GetType().Name);

                pipeline[i].Apply(bufferedFrame);
            }

            Console.WriteLine("Done\n");
        }

        public void Process()
        {
            Console.WriteLine();

            BufferedFrame bufferedFrame = new BufferedFrame(width, height);

            for (int i = 0; i < frames.Length; i++)
            {
                Console.WriteLine("PROCESSING FRAME " + i);

                // TODO: Initial processing

                bufferedFrame.Fill(frames[i]);
                ExecutePipeline(bufferedFrame);
                bufferedFrame.SaveFrame(@"D:\Projects\Animal Detection\Heatmap\", "Frame_", i);
                //heatmap.ToBitmap().Save(@"D:\Projects\Animal Detection\Heatmap\Frame_" + i + ".png");

                // TODO: Final processing
            }
        }

        /*
        public void Process()
        {
            // Should decay slow for high values and fast for low values
            // Should grow slow for low values and fast for high values
            var decay = new Modifiers.Decay.Linear(1 / 100f);
            var growth = new Modifiers.Growth.Linear(1 / 50f);
            var dropoff = new Modifiers.Dropoff.Linear();

            Heatmap heatmap = new Heatmap(frameWidth, frameHeight, decay, growth, dropoff, 6, 1f / frameRate);

            Console.Write("Processing frames".PadRight(30));
            for (int i = 1; i < frames.Length; i++)
            {
                // PASS 1: Update heatmap
                for (int x = 0; x < frameWidth; x++)
                {
                    for (int y = 0; y < frameHeight; y++)
                    {
                        float difference = Math.Abs(frames[i][x,y] - frames[i - 1][x,y]);
                        heatmap.Plot(x, y, difference * 1/50f);
                    }
                }

                heatmap.Step();
                //heatmap.ToBitmap().Save(@"D:\Projects\Animal Detection\Heatmap\Frame_" + i + ".png");
                
                // PASS 2: Calculate weighted difference map
                float[,] map = new float[frameWidth, frameHeight];

                for (int x = 0; x < frameWidth; x++)
                {
                    for (int y = 0; y < frameHeight; y++)
                    {
                        float difference = Math.Abs(frames[i][x, y] - frames[i - 1][x, y]);
                        map[x, y] = (1f - heatmap.map[x, y]) * difference;
                    }
                }

                // Generate and save image
                Bitmap processedFrame = new Bitmap(frameWidth, frameHeight);

                for (int x = 0; x < frameWidth; x++)
                {
                    for (int y = 0; y < frameHeight; y++)
                    {
                        int c = (int)Math.Round(255 * map[x, y]);
                        processedFrame.SetPixel(x, y, Color.FromArgb(c, c, c));
                    }
                }

                processedFrame.Save(@"D:\Projects\Animal Detection\Heatmap\Frame_" + i + ".png");
                
                // Update progress
                if (i % ((int)Math.Floor(frames.Length / 10f)) == 0)
                    Console.Write(".");
            }
            Console.Write(" Done\n");
        }
        */
       
    }
}
