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

        [SlashCommand("halfmast", "Displays whether the U.S. flag is currently at half staff anywhere in the country.")]
        public async Task DisplayHalfMastInformationAsync()
        {
            var halfMastStatus = await _halfMastService.GetHalfMastStatus();

            if (!halfMastStatus.IsStatusKnown)
            {
                var embedBuilder = new EmbedBuilder()
                    .WithTitle("I couldn't verify the current half staff status")
                    .WithDescription("The source page could not be parsed or reached.")
                    .WithFooter($"Source: {halfMastStatus.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await RespondAsync(embed: embedBuilder.Build());

                return;
            }

            if (halfMastStatus.IsHalfStaff)
            {
                var embedBuilder = new EmbedBuilder()
                    .WithTitle("The U.S. flag is at half mast")
                    .WithDescription(halfMastStatus.Description)
                    .WithFooter($"Source: {halfMastStatus.SourceUrl}",
                        $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

                await RespondAsync(embed: embedBuilder.Build());

                return;
            }

            var inactiveEmbedBuilder = new EmbedBuilder()
                .WithTitle("The U.S. flag is not at half staff")
                .WithDescription("No active half staff notices were found.")
                .WithFooter($"Source: {halfMastStatus.SourceUrl}",
                    $"https://raw.githubusercontent.com/flyingfisch/FischBotDiscord-csharp/master/assets/us-flag.png");

            await RespondAsync(embed: inactiveEmbedBuilder.Build());
        }
    }
}
