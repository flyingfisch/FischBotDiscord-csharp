using System;

namespace FischBot.Models.Finance
{
    public class TimeSeriesValue
    {
        public DateTimeOffset Datetime { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}