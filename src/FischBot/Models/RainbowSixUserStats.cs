using System;
using FischBot.Api.StatsDbClient.Dtos;
using RainbowSix = FischBot.Api.StatsDbClient.Dtos.RainbowSix;

namespace FischBot.Models
{
    public class RainbowSixUserStats
    {
        private static readonly string _r6DbBaseUrl = "https://r6db.net";
        private static readonly string _r6RankImageBaseUrl = "https://api.statsdb.net/r6/assets/ranks/";

        public string Id { get; set; }
        public string Nickname { get; set; }
        public TimeSpan TimePlayed { get; set; }
        public string AvatarUrl { get; set; }
        public string StatsUrl { get; set; }

        public int OverallKills { get; set; }
        public int OverallDeaths { get; set; }
        public int OverallAssists { get; set; }
        public int OverallWins { get; set; }
        public int OverallLosses { get; set; }

        public string RankImageUrl { get; set; }
        public int RankedMmr { get; set; }
        public int RankedKills { get; set; }
        public int RankedDeaths { get; set; }
        public int RankedWins { get; set; }
        public int RankedLosses { get; set; }
        public int RankedAbandons { get; set; }

        public int UnrankedMmr { get; set; }
        public int UnrankedKills { get; set; }
        public int UnrankedDeaths { get; set; }
        public int UnrankedWins { get; set; }
        public int UnrankedLosses { get; set; }
        public int UnrankedAbandons { get; set; }


        public int CasualKills { get; set; }
        public int CasualDeaths { get; set; }
        public int CasualWins { get; set; }
        public int CasualLosses { get; set; }

        public static RainbowSixUserStats MapFromStatsDbClientDto(StatsDbApiResponse<RainbowSix.GetUserStatsPayload> getUserStatsDto)
        {
            var userStats = new RainbowSixUserStats()
            {
                Id = getUserStatsDto.Payload.User.Id,
                Nickname = getUserStatsDto.Payload.User.Nickname,
                TimePlayed = TimeSpan.FromSeconds(getUserStatsDto.Payload.Stats.General.TimePlayed),
                StatsUrl = $"{_r6DbBaseUrl}{getUserStatsDto.Payload.User.Url}",
                RankImageUrl = $"{_r6RankImageBaseUrl}{getUserStatsDto.Payload.Stats.Rank.Rank}",
                AvatarUrl = getUserStatsDto.Payload.User.Avatar,

                OverallKills = getUserStatsDto.Payload.Stats.General.Kills,
                OverallDeaths = getUserStatsDto.Payload.Stats.General.Deaths,
                OverallAssists = getUserStatsDto.Payload.Stats.General.Assists,
                OverallWins = getUserStatsDto.Payload.Stats.General.Wins,
                OverallLosses = getUserStatsDto.Payload.Stats.General.Losses,

                RankedMmr = getUserStatsDto.Payload.Stats.Rank.Mmr,
                RankedKills = getUserStatsDto.Payload.Stats.Rank.Kills,
                RankedDeaths = getUserStatsDto.Payload.Stats.Rank.Deaths,
                RankedWins = getUserStatsDto.Payload.Stats.Rank.Wins,
                RankedLosses = getUserStatsDto.Payload.Stats.Rank.Losses,
                RankedAbandons = getUserStatsDto.Payload.Stats.Rank.Abandons,

                UnrankedMmr = getUserStatsDto.Payload.Stats.Rank.Mmr,
                UnrankedKills = getUserStatsDto.Payload.Stats.Rank.Kills,
                UnrankedDeaths = getUserStatsDto.Payload.Stats.Rank.Deaths,
                UnrankedWins = getUserStatsDto.Payload.Stats.Rank.Wins,
                UnrankedLosses = getUserStatsDto.Payload.Stats.Rank.Losses,
                UnrankedAbandons = getUserStatsDto.Payload.Stats.Rank.Abandons,

                CasualKills = getUserStatsDto.Payload.Stats.Casual.Kills,
                CasualDeaths = getUserStatsDto.Payload.Stats.Casual.Deaths,
                CasualWins = getUserStatsDto.Payload.Stats.Casual.Wins,
                CasualLosses = getUserStatsDto.Payload.Stats.Casual.Losses,
            };

            return userStats;
        }
    }
}