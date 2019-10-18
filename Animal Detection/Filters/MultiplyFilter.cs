using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public class MultiplyFilter : Filter
    {

        private float[,] array;

        public MultiplyFilter(float[,] array)
        {
            this.array = array;
        }

        public void Apply(BufferedFrame frame)
        {
            for (int x = 0; x < frame.width; x++)
            {
                for (int y = 0; y < frame.height; y++)
                {
                    float newValue = frame.GetValue(x, y) * array[x, y];
                    frame.SetBufferValue(x, y, newValue);
                }
            }

            frame.Apply();
        }
    }
}