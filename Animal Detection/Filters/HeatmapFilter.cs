using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public class HeatmapFilter : Filter
    {
        private readonly Heatmap heatmap;

        public HeatmapFilter(Heatmap heatmap)
        {
            this.heatmap = heatmap;
        }

        public void Apply(BufferedFrame frame)
        {

            for (int x = 0; x < frame.width; x++)
            {
                for (int y = 0; y < frame.height; y++)
                {
                    heatmap.Plot(x, y, frame.GetValue(x, y));
                }
            }

            heatmap.Step();
        }
    }
}
