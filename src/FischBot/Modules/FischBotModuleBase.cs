using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;

namespace FischBot.Modules
{
    public class FischBotModuleBase<T> : ModuleBase where T : class, ICommandContext
    {
        private readonly IDiscordModuleService _module;

        public FischBotModuleBase(IDiscordModuleService moduleService) : base()
        {
            _module = moduleService;
        }

        protected async Task<IUserMessage> ReplyAsync(
            string message = null,
            bool isTTS = false,
            Embed embed = null,
            RequestOptions options = null,
            AllowedMentions allowedMentions = null,
            MessageReference messageReference = null,
            MessageComponent components = null,
            ISticker[] stickers = null,
            Embed[] embeds = null,
            MessageFlags flags = MessageFlags.None)
        {
            return await _module.ReplyAsync(Context, message, isTTS, embed, options, allowedMentions, messageReference, components, stickers, embeds, flags);
        }
    }
}