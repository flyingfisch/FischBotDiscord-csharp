using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Api;
using FischBot.Services;

namespace FischBot.Modules
{
    public class HalfMastModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IStarsAndStripesDailyClient _starsAndStripesDailyClient;

        public HalfMastModule(IDiscordModuleService moduleService, IStarsAndStripesDailyClient starsAndStripesDailyClient) : base(moduleService)
        {
            _starsAndStripesDailyClient = starsAndStripesDailyClient;
        }

        [Command("halfmast")]
        [Summary("Displays half mast information about the US flag.")]
        public async Task DisplayHalfMastInformationAsync([Remainder][Summary("The date to query for.")] DateTime? date = null)
        {
            var halfMastDate = date ?? DateTime.Today;
            var halfMastInfo = _starsAndStripesDailyClient.GetUsFlagHalfMastInfo(halfMastDate);
            var embedTitle = CreateHalfMastInfoEmbedTitle(halfMastDate, halfMastInfo.FlagIsHalfMast);

            if (halfMastInfo.FlagIsHalfMast)
            {
                var embedField = new EmbedFieldBuilder()
                    .WithName(halfMastInfo.Title)
                    .WithValue($"{halfMastInfo.Description.Substring(0, 500)}...");

                var embed = new EmbedBuilder()
                    .WithTitle(embedTitle)
                    .WithFields(embedField)
                    .WithUrl(halfMastInfo.ReadMoreUrl)
                    .WithFooter(text: "From StarsAndStripesDaily.org. Information not guaranteed to be accurate.",
                                iconUrl: $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await ReplyAsync(embed: embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder()
                    .WithTitle(embedTitle);

                await ReplyAsync(embed: embed.Build());
            }
        }

        private string CreateHalfMastInfoEmbedTitle(DateTime date, bool flagIsHalfMast)
        {
            if (date == DateTime.Today)
            {
                if (flagIsHalfMast)
                {
                    return "The flag is at half mast today.";
                }
                else
                {
                    return "The flag is not at half mast today.";
                }
            }
            else
            {
                if (flagIsHalfMast)
                {
                    return $"The flag was at half mast on {date.ToString("D")}.";
                }
                else
                {
                    return $"The flag was not at half mast on {date.ToString("D")}.";
                }
            }
        }
    }
}