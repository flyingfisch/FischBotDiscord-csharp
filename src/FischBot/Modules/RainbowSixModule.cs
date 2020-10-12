using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Models;
using FischBot.Services;

namespace FischBot.Modules
{
    [Group("siege")]
    public class RainbowSixModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IRainbowSixService _rainbowSixService;

        public RainbowSixModule(IDiscordModuleService moduleService, IRainbowSixService rainbowSixService) : base(moduleService)
        {
            _rainbowSixService = rainbowSixService;
        }

        [Summary("Gets Rainbow Six stats for the specified user.")]
        [Command("stats")]
        public async Task GetSiegeStatsAsync([Summary("Type of stats to display. Valid types: Overall, Ranked, Unranked, Casual.")] RainbowSixStatsType statsType, [Summary("User to get stats for.")] string userName)
        {
            try
            {
                var rainbowSixUserStats = await _rainbowSixService.GetRainbowSixUserStats(userName);

                var embed = new EmbedBuilder()
                    .WithUrl(rainbowSixUserStats.StatsUrl)
                    .WithThumbnailUrl(rainbowSixUserStats.AvatarUrl)
                    .WithFooter("Stats collected from R6DB.net");

                switch (statsType)
                {
                    case RainbowSixStatsType.Ranked:
                        embed
                            .WithTitle($"Rainbow Six Siege Current Season Ranked Statistics for {userName}")
                            .AddField("MMR", rainbowSixUserStats.RankedMmr, inline: true)
                            .AddField("Kills", rainbowSixUserStats.RankedKills, inline: true)
                            .AddField("Deaths", rainbowSixUserStats.RankedDeaths, inline: true)
                            .AddField("Wins", rainbowSixUserStats.RankedWins, inline: true)
                            .AddField("Losses", rainbowSixUserStats.RankedLosses, inline: true);

                        break;
                    case RainbowSixStatsType.Unranked:
                        embed
                            .WithTitle($"Rainbow Six Siege Current Season Unranked Statistics for {userName}")
                            .AddField("MMR", rainbowSixUserStats.UnrankedMmr, inline: true)
                            .AddField("Kills", rainbowSixUserStats.UnrankedKills, inline: true)
                            .AddField("Deaths", rainbowSixUserStats.UnrankedDeaths, inline: true)
                            .AddField("Wins", rainbowSixUserStats.UnrankedWins, inline: true)
                            .AddField("Losses", rainbowSixUserStats.UnrankedLosses, inline: true);

                        break;
                    case RainbowSixStatsType.Casual:
                        embed
                            .WithTitle($"Rainbow Six Siege Casual Statistics for {userName}")
                            .AddField("Kills", rainbowSixUserStats.CasualKills, inline: true)
                            .AddField("Deaths", rainbowSixUserStats.CasualDeaths, inline: true)
                            .AddField("Wins", rainbowSixUserStats.CasualWins, inline: true)
                            .AddField("Losses", rainbowSixUserStats.CasualLosses, inline: true);

                        break;
                    default:
                        embed
                            .WithTitle($"Rainbow Six Siege Statistics for {userName}")
                            .AddField("Time Played", $"{Math.Round(rainbowSixUserStats.TimePlayed.TotalHours, 0)}h", inline: false)
                            .AddField("Kills", rainbowSixUserStats.OverallKills, inline: true)
                            .AddField("Deaths", rainbowSixUserStats.OverallDeaths, inline: true)
                            .AddField("Wins", rainbowSixUserStats.OverallWins, inline: true)
                            .AddField("Losses", rainbowSixUserStats.OverallLosses, inline: true);

                        break;
                }


                await ReplyAsync(embed: embed.Build());
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }
    }
}