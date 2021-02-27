using System.Threading.Tasks;
using AlphaVantage.Net.Crypto;
using AlphaVantage.Net.Stocks;

namespace FischBot.Services.AlphaVantageService
{
    public interface IAlphaVantageService
    {
        Task<GlobalQuote> GetCurrentStockQuote(string stockSymbol);
        Task<CryptoExchangeRate> GetCurrentCryptoExchangeRate(string cryptoSymbol, string currencySymbol);
    }
}