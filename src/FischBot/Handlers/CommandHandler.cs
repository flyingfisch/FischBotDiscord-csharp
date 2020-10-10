using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FischBot.Handlers
{
    public class CommandHandler
    {
        private readonly IConfiguration _configuration;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discordClient;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _configuration = services.GetRequiredService<IConfiguration>();
            _commands = services.GetRequiredService<CommandService>();
            _discordClient = services.GetRequiredService<DiscordSocketClient>();

            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discordClient.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Author.Id == _discordClient.CurrentUser.Id || message.Author.IsBot)
            {
                return;
            }


            var commandStartPosition = 0;
            var commandPrefix = _configuration.GetSection("FischBot:commandPrefix").Value;

            if (message.HasStringPrefix(commandPrefix, ref commandStartPosition) ||
                message.HasMentionPrefix(_discordClient.CurrentUser, ref commandStartPosition))
            {
                var context = new SocketCommandContext(_discordClient, message);

                await _commands.ExecuteAsync(context, commandStartPosition, _services);
            }
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                Console.WriteLine($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
            }
            else if (result.IsSuccess)
            {
                Console.WriteLine($"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
            }
            else
            {
                await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
            }
        }
    }
}