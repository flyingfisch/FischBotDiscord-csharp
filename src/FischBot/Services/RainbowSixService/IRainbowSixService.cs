using System.Threading.Tasks;
using FischBot.Models;

namespace FischBot.Services.RainbowSixService
{
    public interface IRainbowSixService
    {
        Task<RainbowSixUserStats> GetRainbowSixUserStats(string userName);
    }
}