using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MovementDetection
{
    public class Floodfill
    {

        public int[,] Apply(int[,] map)
        {
            // Create the floodmap initialized to zero
            int[,] floodMap = new int[map.GetLength(0), map.GetLength(1)];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    floodMap[x, y] = -1;
                }
            }

            Queue<(int x, int y)> startingPoints = new Queue<(int x, int y)>();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] != 0)
                        startingPoints.Enqueue((x, y));
                }
            }

            // Create the regions to hold the different floodfilled areas
            int region = 0;
            List<HashSet<(int x, int y)>> regions = new List<HashSet<(int x, int y)>>();

            // Create the queue and add the start position found earlier
            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

            while (startingPoints.Count > 0)
            {
                (int startX, int startY) = startingPoints.Dequeue();

                if (floodMap[startX, startY] != -1 || map[startX, startY] == 0)
                    continue;

                region++;
                queue.Enqueue((startX, startY));

                while (queue.Count > 0)
                {
                    (int x, int y) = queue.Dequeue();

                    if (floodMap[x, y] != -1 || map[x, y] == 0)
                        continue;

                    floodMap[x, y] = region;

                    if (x < map.GetLength(0) - 1 && map[x + 1, y] == 1)
                        queue.Enqueue((x + 1, y));
                    if (x > 0 && map[x - 1, y] == 1)
                        queue.Enqueue((x - 1, y));
                    if (y < map.GetLength(1) - 1 && map[x, y + 1] == 1)
                        queue.Enqueue((x, y + 1));
                    if (y > 0 && map[x, y - 1] == 1)
                        queue.Enqueue((x, y - 1));
                }
            }

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (floodMap[x, y] == -1)
                        floodMap[x, y] = 0;
                }
            }

            return floodMap;
        }
    }
}
