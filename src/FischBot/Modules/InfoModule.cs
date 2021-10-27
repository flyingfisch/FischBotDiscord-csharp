using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using Microsoft.Extensions.Configuration;

namespace FischBot.Modules
{
    public class InfoModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly IConfiguration _configuration;

        public InfoModule(IDiscordModuleService moduleService, CommandService commands, IConfiguration configuration) : base(moduleService)
        {
            _commands = commands;
            _configuration = configuration;
        }

        [Command("say")]
        [Summary("Echoes a message.")]
        public async Task SayAsync([Remainder][Summary("The text to echo.")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("good bot")]
        [Summary("Gives the bot some praise.")]
        public async Task ReceivePraiseAsync()
        {
            await ReplyAsync("😊");
        }

        [Command("help")]
        [Alias("commands")]
        [Summary("Displays a list of supported commands and their descriptions. When a command is specified, gets help for that command.")]
        public async Task DisplayHelpAsync([Remainder][Summary("Name of the command to get help for.")] string commandName = null)
        {
            var commandPrefix = _configuration.GetSection("FischBot:commandPrefix").Value;

            if (commandName is null)
            {
                // if commandName is null, display a list of commands and command summaries

                var commandFields = _commands.Commands
                    .Select(command => new EmbedFieldBuilder()
                        .WithName($"{commandPrefix}{(command.Module.Group != null ? $"{command.Module.Group} " : "")}{command.Name}")
                        .WithValue($"{command.Summary}"));

                var embed = new EmbedBuilder()
                    .WithTitle("FischBot Supported Commands")
                    .WithFields(commandFields)
                    .WithFooter("View my source code on Github! ♥ https://github.com/flyingfisch/FischBotDiscord-csharp");

                await ReplyAsync(embed: embed.Build());
            }
            else
            {
                // if commandName is not null, display command usage information

                var command = _commands.Commands.Where(c => c.Aliases.Contains(commandName.Replace(commandPrefix, string.Empty))).FirstOrDefault();
                // the command name is included in the list of Aliases, so we only need to check there

                if (command is null)
                {
                    await ReplyAsync("Command does not exist.");
                }
                else
                {
                    var commandAliases = command.Aliases.Where(a => a != $"{(command.Module.Group != null ? $"{command.Module.Group} " : "")}{command.Name}");
                    var parameterNames = command.Parameters.Select(p => $"[{p.Name}]");
                    var parameterSummaries = command.Parameters.Select(p => $"`{p.Name}`{(p.IsOptional ? " (optional)" : string.Empty)}: {p.Summary}");

                    var usageString = $"`{commandPrefix}";

                    if (!string.IsNullOrWhiteSpace(command.Module.Group))
                    {
                        usageString += command.Module.Group + " ";
                    }

                    usageString += $"{command.Name} {string.Join(' ', parameterNames)}`";

                    var embed = new EmbedBuilder()
                        .WithTitle($"Help: {command.Name}")
                        .AddField("Summary", command.Summary)
                        .AddField("Usage", usageString)
                        .AddField("Parameters", string.Join('\n', parameterSummaries))
                        .AddField("Aliases", commandAliases.Any() ? string.Join(", ", commandAliases) : "None");

                    await ReplyAsync(embed: embed.Build());
                }
            }
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