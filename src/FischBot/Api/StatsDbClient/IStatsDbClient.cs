using System.Threading.Tasks;
using FischBot.Api.StatsDbClient.Dtos;

namespace FischBot.Api.StatsDbClient
{
    public interface IStatsDbClient
    {
        Task<StatsDbApiResponse<Dtos.RainbowSix.SearchUsersPayload>> SearchRainbowSixUsers(string userName, string platform);
        Task<StatsDbApiResponse<Dtos.RainbowSix.GetUserStatsPayload>> GetRainbowSixUserStats(string userId);
    }
}