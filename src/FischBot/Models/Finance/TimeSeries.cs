namespace FischBot.Models.Finance
{
    public class TimeSeries
    {
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }

        public TimeSeriesValue[] Values { get; set; }
    }
}