using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace FischBot.Services
{
    public class DiscordModuleService : IDiscordModuleService
    {
        public async Task<IUserMessage> ReplyAsync(ICommandContext context, string text = null, bool isTTS = false,
            Embed embed = null, RequestOptions options = null)
        {
            return await context.Channel.SendMessageAsync(text, isTTS, embed, options).ConfigureAwait(false);
        }
    }
}