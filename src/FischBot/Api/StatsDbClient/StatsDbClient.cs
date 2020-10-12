using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Api.StatsDbClient.Dtos;
using RainbowSix = FischBot.Api.StatsDbClient.Dtos.RainbowSix;

namespace FischBot.Api.StatsDbClient
{
    public class StatsDbClient : IStatsDbClient
    {
        private readonly HttpClient _httpClient;

        private readonly string _statsDbBaseUrl = "https://api.statsdb.net/";

        public StatsDbClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StatsDbApiResponse<RainbowSix.SearchUsersPayload>> SearchRainbowSixUsers(string userName, string platform)
        {
            var response = await _httpClient.GetAsync($"{_statsDbBaseUrl}r6/search/{platform}/{userName}/slow");

            response.EnsureSuccessStatusCode();
            var deserializedResponse = await response.Content.ReadAsAsync<StatsDbApiResponse<RainbowSix.SearchUsersPayload>>();

            return deserializedResponse;
        }

        public async Task<StatsDbApiResponse<RainbowSix.GetUserStatsPayload>> GetRainbowSixUserStats(string userId)
        {
            var response = await _httpClient.GetAsync($"{_statsDbBaseUrl}r6/player/{userId}");

            response.EnsureSuccessStatusCode();
            var deserializedResponse = await response.Content.ReadAsAsync<StatsDbApiResponse<RainbowSix.GetUserStatsPayload>>();

            return deserializedResponse;
        }
    }
}