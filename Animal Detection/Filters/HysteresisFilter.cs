using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public class HysteresisFilter : Filter
    {
        private static readonly (int x, int y)[] immediateNeighbours = {(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)};

        private const float UNVISITED = -1;
        private const float BLANK = 0;
        private const float FILLED = 1;

        private readonly float lowThreshold, highThreshold;
        
        private Queue<(int x, int y)> queue;

        public HysteresisFilter(float lowThreshold, float highThreshold)
        {
            this.lowThreshold = lowThreshold;
            this.highThreshold = highThreshold;

            queue = new Queue<(int x, int y)>();
        }

        public void Apply(BufferedFrame frame)
        {
            // Set all pixels above the low threshold to UNVISITED
            // These will be visited later to see if they are a
            // high threshold pixel or connected to one
            for (int x = 0; x < frame.width; x++)
            {
                for (int y = 0; y < frame.height; y++)
                {
                    if (frame.GetValue(x, y) > lowThreshold)
                        frame.SetBufferValue(x, y, UNVISITED);
                    else
                        frame.SetBufferValue(x, y, BLANK);
                }
            }

            // Enqueue the high threshold pixel starting points
            for (int x = 0; x < frame.width; x++)
            {
                for (int y = 0; y < frame.height; y++)
                {
                    if (frame.GetValue(x, y) > highThreshold)
                        queue.Enqueue((x, y));
                }
            }
            
            while (queue.Count > 0)
            {
                (int x, int y) = queue.Dequeue();

                // Ignore the pixel if it has been visited
                if (frame.GetBufferValue(x, y) != UNVISITED)
                    continue;

                // The queue started with only high threshold pixels. So
                // any low threshold pixel was added as a part of the
                // 'recursive' process (the code below in the foreah)
                if (frame.GetValue(x, y) > lowThreshold)
                    frame.SetBufferValue(x, y, FILLED);
                else
                    frame.SetBufferValue(x, y, BLANK);

                // Go through the immediate neighbours seeing if they can be
                // traversed too
                foreach ((int offsetX, int offsetY) in immediateNeighbours)
                {
                    int currentX = x + offsetX;
                    int currentY = y + offsetY;

                    if (!frame.IsOutOfBounds(currentX, currentY) && frame.GetValue(currentX, currentY) > lowThreshold)
                        queue.Enqueue((currentX, currentY));
                }
            }

            // Any pixel not visited now isn't connected to a high threshold
            // pixel and is therefore, blank.
            for (int x = 0; x < frame.width; x++)
            {
                for (int y = 0; y < frame.height; y++)
                {
                    if (frame.GetBufferValue(x, y) == UNVISITED)
                        frame.SetBufferValue(x, y, BLANK);
                }
            }

            frame.Apply();
        }
    }
}
