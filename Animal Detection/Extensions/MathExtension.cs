using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementDetection.Extensions
{
    public static class MathExtension
    {

        /// <summary>
        /// Wraps the value around if is less than min or greater than max.
        /// e.g. Wrap(-1, 0, 10) == 9
        /// e.g. Wrap(11, 0, 10) == 1
        /// </summary>
        public static int Wrap(int value, int min, int max)
        {
            if (value < 0)
                value += (int)Math.Ceiling(value / (double)(max - 1)) * (max - 1);

            return value % max;
        }

        /// <summary>
        /// Clamps the value between the min and max. Maybe extend the Math class
        /// if this ends up being used elsewhere.
        /// </summary>
        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(Math.Min(value, max - 1), 0);
        }
    }
}
