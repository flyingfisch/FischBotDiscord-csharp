using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Models.Finance;
using Microsoft.Extensions.Configuration;
using TwelveDataSharp;

namespace FischBot.Services.FinanceService
{
    public class FinanceService : IFinanceService
    {
        private readonly IConfiguration _configuration;
        private readonly string _twelveDataApiKey;
        private readonly TwelveDataClient _twelveDataClient;
        private readonly HttpClient _httpClient;

        public FinanceService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;

            _twelveDataApiKey = configuration.GetSection("FischBot:twelveDataApiKey").Value;
            _twelveDataClient = new TwelveDataClient(_twelveDataApiKey, httpClient);

            _httpClient = httpClient;
        }

        public async Task<decimal> GetRealTimeStockPrice(string symbol)
        {
            var response = await _twelveDataClient.GetRealTimePriceAsync(symbol);

            return (decimal)response.Price;
        }

        public async Task<TimeSeries> GetTimeSeries(string symbol, string interval)
        {
            var response = await _twelveDataClient.GetTimeSeriesAsync(symbol, interval);

            var timeSeries = new TimeSeries()
            {
                Symbol = response.Symbol,
                Type = response.Type,
                Currency = response.Currency
            };

            timeSeries.Values = response.Values
                .Select(value => new TimeSeriesValue()
                {
                    Open = (decimal)value.Open,
                    Close = (decimal)value.Close,
                    High = (decimal)value.High,
                    Low = (decimal)value.Low,
                    Volume = value.Volume,
                })
                .ToArray();

            return timeSeries;
        }

        public async Task<TwelveDataUsageStats> GetApiUsageStats()
        {
            var response = await _httpClient.GetAsync($"https://api.twelvedata.com/api_usage?apikey={_twelveDataApiKey}");

            var stats = await response.Content.ReadAsAsync<TwelveDataUsageStats>();

            return stats;
        }
    }
}