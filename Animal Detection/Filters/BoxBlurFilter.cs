using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public class BoxBlurFilter : Filter
    {
        private readonly int passes;
        private readonly int smallWidth, sampleSize;

        public BoxBlurFilter(int smallWidth, int passes)
        {
            this.smallWidth = smallWidth;
            this.passes = passes;

            sampleSize = (smallWidth * 2 + 1) * (smallWidth * 2 + 1);
        }

        public void Apply(BufferedFrame frame)
        {
            for (int p = 0; p < passes; p++)
            {
                for (int x = 0; x < frame.width; x++)
                {
                    for (int y = 0; y < frame.height; y++)
                    {
                        float average = 0f;

                        for (int offsetX = -smallWidth; offsetX <= smallWidth; offsetX++)
                        {
                            for (int offsetY = -smallWidth; offsetY <= smallWidth; offsetY++)
                            {
                                average += frame.GetValue(x + offsetX, y + offsetY, BufferedFrame.OutOfBoundsGetMode.Extend);
                            }
                        }

                        average /= (float)sampleSize;

                        frame.SetBufferValue(x, y, average);
                    }
                }

                frame.Apply();
            }
        }
    }
}
