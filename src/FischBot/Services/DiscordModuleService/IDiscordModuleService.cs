using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FischBot.Services.DiscordModuleService
{
    public interface IDiscordModuleService
    {
        Task<IUserMessage> ReplyAsync(
            ICommandContext context,
            string text = null,
            bool isTTS = false,
            Embed embed = null,
            RequestOptions options = null,
            AllowedMentions allowedMentions = null,
            MessageReference messageReference = null);
    }
}