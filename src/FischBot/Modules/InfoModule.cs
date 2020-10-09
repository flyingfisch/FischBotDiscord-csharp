using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;

namespace Fischbot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly IConfiguration _configuration;

        public InfoModule(CommandService commands, IConfiguration configuration)
        {
            _commands = commands;
            _configuration = configuration;
        }

        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task Say([Remainder][Summary("The text to echo.")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("help")]
        [Summary("Displays a list of supported commands and their descriptions")]
        public async Task DisplayHelp()
        {
            var commandPrefix = _configuration.GetSection("FischBot:commandPrefix").Value;
            var commands = _commands.Commands.ToList();

            var commandFields = commands
                .Select(command => new EmbedFieldBuilder()
                    .WithName($"{commandPrefix}{command.Name}")
                    .WithValue(command.Summary));

            var embed = new EmbedBuilder()
                .WithTitle("FischBot Supported Commands")
                .WithFields(commandFields)
                .WithFooter("View my source code on Github! ♥ https://github.com/flyingfisch/FischBotDiscord-csharp");

            await ReplyAsync(embed: embed.Build());
        }
    }
}