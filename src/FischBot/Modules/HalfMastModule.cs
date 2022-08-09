using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.HalfMastService;

namespace FischBot.Modules
{
    public class HalfMastModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private readonly IHalfMastService _halfMastService;

        public HalfMastModule(IDiscordModuleService moduleService, IHalfMastService halfMastService) : base(moduleService)
        {
            _halfMastService = halfMastService;
        }

        [SlashCommand("halfmast", "Displays latest half mast information about the US flag.")]
        public async Task DisplayHalfMastInformationAsync([Summary(description: "The state to show half mast alerts for. (optional, returns nationwide alerts if unspecified)")] string state = null)
        {
            var latestHalfMastNotice = await _halfMastService.GetLatestHalfMastNotice(state);

            if (latestHalfMastNotice is null) 
            {
                if (string.IsNullOrEmpty(state))
                {
                    await RespondAsync($"I couldn't find any recent half mast alerts for the entire US.");
                }
                else
                {
                    await RespondAsync($"I couldn't find any recent half mast alerts for {Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(state)}.");
                }
            }
            else 
            {
                var embedTitle = CreateHalfMastInfoEmbedTitle(latestHalfMastNotice.State);
                var embedBuilder = new EmbedBuilder()
                    .WithTitle(embedTitle)
                    .WithDescription(latestHalfMastNotice.Description)
                    .WithFooter($"Source: {latestHalfMastNotice.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await RespondAsync(embed: embedBuilder.Build());
            }
        }

        private string CreateHalfMastInfoEmbedTitle(string state)
        {
            if (string.IsNullOrEmpty(state)) 
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