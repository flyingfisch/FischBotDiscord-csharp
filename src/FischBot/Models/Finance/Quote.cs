using System;

namespace FischBot.Models.Finance
{
    public class Quote
    {
        public decimal PercentChange { get; set; }
        public decimal Change { get; set; }
        public decimal PreviousClose { get; set; }
        public long Volume { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Close { get; set; }
        public decimal Open { get; set; }
        public DateTimeOffset Datetime { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}