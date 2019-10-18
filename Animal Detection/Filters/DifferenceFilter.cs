using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public class DifferenceFilter : Filter
    {
        private float[,] previousFrame;

        public void Apply(BufferedFrame frame)
        {
            if (previousFrame == null)
                previousFrame = new float[frame.width, frame.height];

            for (int x = 0; x < frame.width; x++)
            {
                for (int y = 0; y < frame.height; y++)
                {
                    float difference = Math.Abs(frame.GetValue(x, y) - previousFrame[x, y]);
                    frame.SetBufferValue(x, y, difference);
                }
            }

            // Save the frame before the changes have been applied
            frame.WriteToArray(previousFrame);

            frame.Apply();
        }
    }
}
