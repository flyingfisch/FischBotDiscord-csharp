using System;
using System.Linq;
using System.Reflection;
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

        [Command("good bot")]
        [Summary("Gives the bot some praise.")]
        public async Task ReceivePraise()
        {
            await ReplyAsync("😊");
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

        [Command("version")]
        [Summary("Displays version information")]
        public async Task DisplayVersionInformation()
        {
            var informationalVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            var assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;

            var embed = new EmbedBuilder()
                .WithTitle("FischBot Version and Runtime Information")
                .AddField("Version", informationalVersion)
                .AddField("Assembly Version", assemblyVersion.ToString())
                .AddField("Operating System", Environment.OSVersion)
                .AddField("64-Bit OS", Environment.Is64BitOperatingSystem)
                .WithFooter("View my source code on Github! ♥ https://github.com/flyingfisch/FischBotDiscord-csharp");


            await ReplyAsync(embed: embed.Build());
        }
    }
}