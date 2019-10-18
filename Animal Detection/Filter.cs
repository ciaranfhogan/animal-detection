using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovementDetection.Pipeline;

namespace MovementDetection.Filters
{
    public interface Filter
    {
        /// <summary>
        /// Modifies the buffer in some way and then applies the changes.
        /// </summary>
        /// <param name="frame"></param>
        void Apply(BufferedFrame frame);
    }

    /// <summary>
    /// You must call Filter.Release() when done with a filter.
    /// </summary>
    public class FilterNotReleasedException : Exception
    {
        public FilterNotReleasedException() { }

        public FilterNotReleasedException(string message) : base(message) { }

        public FilterNotReleasedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
