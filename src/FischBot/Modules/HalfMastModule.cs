using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Api.StarsAndStripesDailyClient;
using FischBot.Services.DiscordModuleService;

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
        [Alias("halfstaff")]
        [Summary("Displays half mast information about the US flag. When no date is provided, gets information for today.")]
        public async Task DisplayHalfMastInformationAsync([Remainder][Summary("The date to query for. (optional)")] DateTime? date = null)
        {
            var halfMastDate = date ?? DateTime.Today;
            var halfMastInfo = _starsAndStripesDailyClient.GetUsFlagHalfMastInfo(halfMastDate);
            var embedTitle = CreateHalfMastInfoEmbedTitle(halfMastDate, halfMastInfo.FlagIsHalfMast);
            var embedBuilder = new EmbedBuilder()
                .WithTitle(embedTitle)
                .WithFooter("From StarsAndStripesDaily.org. Information not guaranteed to be accurate.",
                    $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

            if (halfMastInfo.FlagIsHalfMast)
            {
                var embedField = new EmbedFieldBuilder()
                    .WithName(halfMastInfo.Title)
                    .WithValue($"{halfMastInfo.Description.Substring(0, 500)}...\n[Full Article]({halfMastInfo.ArticleUrl})");

                embedBuilder.AddField(embedField);
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }

        private string CreateHalfMastInfoEmbedTitle(DateTime date, bool flagIsHalfMast)
        {
            if (date == DateTime.Today)
            {
                return flagIsHalfMast ? "The flag is at half mast today." : "The flag is not at half mast today.";
            }

            return flagIsHalfMast ? $"The flag was at half mast on {date:D}." : $"The flag was not at half mast on {date:D}.";
        }
    }
}