using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vinva.Gastronomy.Common.Models
{
    public record TimeRange
    {
        public double MinSeconds { get; private set; }
        public double MaxSeconds { get; private set; }

        public TimeRange(double minSeconds, double maxSeconds)
        {
            if (minSeconds < 0)
                throw new ArgumentException("Min seconds must be large or equals zero");
            if (maxSeconds < minSeconds)
                throw new ArgumentException("Max seconds must be large of min seconds");
            MinSeconds = minSeconds;
            MaxSeconds = maxSeconds;
        }

        public TimeRange(double seconds) : this(seconds, seconds) { }

        public double GetMinMinutes() => TimeSpan.FromSeconds(MinSeconds).TotalMinutes;
        public double GetMaxMinutes() => TimeSpan.FromSeconds(MaxSeconds).TotalMinutes;

        private static TimeRange _empty = new TimeRange(0, 0);
        public static TimeRange Empty => _empty;       
    }
}
