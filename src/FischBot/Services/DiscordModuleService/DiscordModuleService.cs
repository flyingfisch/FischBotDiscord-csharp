using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FischBot.Services.DiscordModuleService
{
    public class DiscordModuleService : IDiscordModuleService
    {
        public async Task<IUserMessage> ReplyAsync(
            ICommandContext context,
            string text = null,
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
            return await context.Channel.SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference, components, stickers, embeds, flags).ConfigureAwait(false);
        }
    }
}