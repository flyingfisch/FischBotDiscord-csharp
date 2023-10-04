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
                var embedTitle = CreateHalfMastInfoEmbedTitle(halfMastStatus.State, halfMastStatus.IsHalfStaff);
                var embedBuilder = new EmbedBuilder()
                    .WithTitle(embedTitle)
                    .WithDescription(halfMastStatus.Description)
                    .WithFooter($"Source: {halfMastStatus.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await RespondAsync(embed: embedBuilder.Build());
            }
            else
            {
                var embedTitle = CreateHalfMastInfoEmbedTitle(halfMastStatus.State, halfMastStatus.IsHalfStaff);
                var embedBuilder = new EmbedBuilder()
                    .WithTitle(embedTitle)
                    .WithFooter($"Source: {halfMastStatus.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await RespondAsync(embed: embedBuilder.Build());
            }
        }

        private string CreateHalfMastInfoEmbedTitle(string state, bool isHalfMast)
        {
            if (isHalfMast)
            {
                if (string.IsNullOrEmpty(state))
                {
                    return $"The flag is at half mast";
                }
                else
                {
                    return $"The flag is at half mast in {state}";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(state))
                {
                    return $"The flag is not at half mast";
                }
                else
                {
                    return $"The flag is not at half mast in {state}";
                }
            }
        }
    }
}