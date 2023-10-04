using System;
using System.Linq;
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
        public async Task DisplayHalfMastInformationAsync([Summary(description: "The state to show half mast alerts for. (optional, returns nationwide alerts if unspecified)")] string stateAbbreviation = null)
        {
            var halfMastStatus = await _halfMastService.GetHalfMastStatus(stateAbbreviation);

            if (halfMastStatus.IsHalfStaff)
            {
                var embedTitle = CreateHalfMastInfoEmbedTitle(halfMastStatus.State);
                var embedBuilder = new EmbedBuilder()
                    .WithTitle(embedTitle)
                    .WithDescription(halfMastStatus.Description)
                    .WithFooter($"Source: {halfMastStatus.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await RespondAsync(embed: embedBuilder.Build());
            }
            else
            {
                if (string.IsNullOrEmpty(stateAbbreviation))
                {
                    await RespondAsync($"I couldn't find any recent half mast alerts for the entire US.");
                }
                else
                {
                    await RespondAsync($"I couldn't find any recent half mast alerts for {stateAbbreviation.ToUpper()}.");
                }
            }
        }

        private string CreateHalfMastInfoEmbedTitle(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                return "The flag is at half mast";
            }
            else
            {
                return $"The flag is at half mast in {state}";
            }
        }
    }
}