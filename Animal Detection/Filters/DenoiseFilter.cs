using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public class DenoiseFilter : Filter
    {
        private readonly int smallWidth;
        private readonly int threshold;
        private readonly int passes;

        public DenoiseFilter(int smallWidth, int threshold, int passes)
        {
            this.smallWidth = smallWidth;
            this.threshold = threshold;
            this.passes = passes;
        }

        public void Apply(BufferedFrame frame)
        {
            for (int p = 0; p < passes; p++)
            {
                for (int x = 0; x < frame.width; x++)
                {
                    for (int y = 0; y < frame.height; y++)
                    {
                        float sum = 0;

                        for (int offsetX = -smallWidth; offsetX <= smallWidth; offsetX++)
                        {
                            for (int offsetY = -smallWidth; offsetY <= smallWidth; offsetY++)
                            {
                                sum += frame.GetValue(x + offsetX, y + offsetY, BufferedFrame.OutOfBoundsGetMode.Extend);
                            }
                        }

                        if (sum > threshold)
                            frame.SetBufferValue(x, y, 1);
                        else
                            frame.SetBufferValue(x, y, 0);
                    }
                }

                frame.Apply();
            }
        }
    }
}
