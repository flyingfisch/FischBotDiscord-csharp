using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FischBot.Services.DiscordModuleService
{
    public interface IDiscordModuleService
    {
        Task RespondAsync(
            IInteractionContext context,
            string text = null,
            Embed[] embeds = null, 
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions allowedMentions = null,
            RequestOptions options = null,
            MessageComponent components = null,
            Embed embed = null);

        Task<IUserMessage> ReplyAsync(
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
            MessageFlags flags = MessageFlags.None);
    }
}