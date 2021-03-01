using System.Threading.Tasks;
using FischBot.Models.Finance;

namespace FischBot.Services.FinanceService
{
    public interface IFinanceService
    {
        Task<decimal> GetRealTimeStockPrice(string symbol);
        Task<TimeSeries> GetTimeSeries(string symbol, string interval);
        Task<TwelveDataUsageStats> GetApiUsageStats();
    }
}