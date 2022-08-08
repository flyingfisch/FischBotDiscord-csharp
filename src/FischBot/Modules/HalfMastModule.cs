using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.HalfMastService;

namespace FischBot.Modules
{
    public class HalfMastModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IHalfMastService _halfMastService;

        public HalfMastModule(IDiscordModuleService moduleService, IHalfMastService halfMastService) : base(moduleService)
        {
            _halfMastService = halfMastService;
        }

        [Command("halfmast")]
        [Alias("halfstaff")]
        [Summary("Displays latest half mast information about the US flag. By default it shows nationwide half mast alerts, but if a state is specified it will show the latest local half mast alert instead.")]
        public async Task DisplayHalfMastInformationAsync([Remainder][Summary("The state to show half mast alerts for. (optional)")] string state = "Entire United States")
        {
            var latestHalfMastNotice = await _halfMastService.GetLatestHalfMastNotice(state);

            if (latestHalfMastNotice is null) 
            {
                await ReplyAsync("I couldn't find any recent half mast alerts.");
            }
            else 
            {
                var embedTitle = CreateHalfMastInfoEmbedTitle(latestHalfMastNotice.State);
                var embedBuilder = new EmbedBuilder()
                    .WithTitle(embedTitle)
                    .WithDescription(latestHalfMastNotice.Description)
                    .WithFooter($"Source: {latestHalfMastNotice.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await ReplyAsync(embed: embedBuilder.Build());
            }
        }

        private string CreateHalfMastInfoEmbedTitle(string state)
        {
            if (state.ToLower() == "entire united states") 
            {
                return "Latest Half Mast Alert";
            }
            else 
            {
                return $"Latest Half Mast Alert for {state}";
            }
        }
    }
}