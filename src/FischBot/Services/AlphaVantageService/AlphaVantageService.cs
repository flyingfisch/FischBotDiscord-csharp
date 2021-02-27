using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaVantage.Net.Common.Currencies;
using AlphaVantage.Net.Common.Intervals;
using AlphaVantage.Net.Core.Client;
using AlphaVantage.Net.Crypto;
using AlphaVantage.Net.Crypto.Client;
using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.Client;
using Microsoft.Extensions.Configuration;

namespace FischBot.Services.AlphaVantageService
{
    public class AlphaVantageService : IAlphaVantageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _alphaVantageApiKey;
        private readonly AlphaVantageClient _alphaVantageClient;
        private readonly StocksClient _stocksClient;
        private readonly CryptoClient _cryptoClient;

        public AlphaVantageService(IConfiguration configuration)
        {
            _configuration = configuration;

            _alphaVantageApiKey = configuration.GetSection("FischBot:alphaVantageApiKey").Value;

            _alphaVantageClient = new AlphaVantageClient(_alphaVantageApiKey);
            _stocksClient = _alphaVantageClient.Stocks();
            _cryptoClient = _alphaVantageClient.Crypto();
        }

        public async Task<GlobalQuote> GetCurrentStockQuote(string stockSymbol)
        {
            var response = await _stocksClient.GetGlobalQuoteAsync(stockSymbol);

            return response;
        }

        public async Task<CryptoExchangeRate> GetCurrentCryptoExchangeRate(string cryptoSymbol, string currencySymbol)
        {
            var digitalCurrency = (DigitalCurrency)Enum.Parse(typeof(DigitalCurrency), cryptoSymbol);
            var physicalCurrency = (PhysicalCurrency)Enum.Parse(typeof(PhysicalCurrency), currencySymbol);

            var response = await _cryptoClient.GetExchangeRateAsync(digitalCurrency, physicalCurrency);

            return response;
        }
    }
}