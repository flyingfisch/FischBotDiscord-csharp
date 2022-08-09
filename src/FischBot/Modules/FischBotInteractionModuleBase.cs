using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class FischBotInteractionModuleBase<T> : InteractionModuleBase where T : class, IInteractionContext
    {
        private readonly IDiscordModuleService _moduleService;

        public FischBotInteractionModuleBase(IDiscordModuleService moduleService) : base()
        {
            _moduleService = moduleService;
        }

        protected override async Task RespondAsync(
            string text = null,
            Embed[] embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions allowedMentions = null,
            RequestOptions options = null,
            MessageComponent components = null,
            Embed embed = null)
        {
            await _moduleService.RespondAsync(Context, text, embeds, isTTS, ephemeral, allowedMentions, options, components, embed);
        }
    }
}
