using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FischBot.Services.DiscordModuleService
{
    public class DiscordModuleService : IDiscordModuleService
    {
        public async Task RespondAsync(
            IInteractionContext context,
            string text = null,
            Embed[] embeds = null, 
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions allowedMentions = null,
            RequestOptions options = null,
            MessageComponent components = null,
            Embed embed = null)
        {
            await context.Interaction.RespondAsync(text, embeds, isTTS, ephemeral, allowedMentions, components, embed, options).ConfigureAwait(false);
        }

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