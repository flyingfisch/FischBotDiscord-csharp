using System;
using System.Threading.Tasks;
using FischBot.Api.StatsDbClient;
using FischBot.Models;

namespace FischBot.Services
{
    public class RainbowSixService : IRainbowSixService
    {
        private readonly IStatsDbClient _statsDbClient;

        public RainbowSixService(IStatsDbClient statsDbClient)
        {
            _statsDbClient = statsDbClient;
        }

        public async Task<RainbowSixUserStats> GetRainbowSixUserStats(string userName)
        {
            // find the user so we can get their ID.

            var searchResultsDto = await _statsDbClient.SearchRainbowSixUsers(userName, "pc");

            if (searchResultsDto.Count == 0 || searchResultsDto.Payload is null || !searchResultsDto.Payload.Exists)
            {
                // TODO: Throw a more specific exception or make a new exception type for this
                throw new Exception($"No user exists with username {userName}");
            }


            // get the user's stats by ID.

            var getUserStatsDto = await _statsDbClient.GetRainbowSixUserStats(searchResultsDto.Payload.User.Id);

            return RainbowSixUserStats.MapFromStatsDbClientDto(getUserStatsDto);
        }
    }
}