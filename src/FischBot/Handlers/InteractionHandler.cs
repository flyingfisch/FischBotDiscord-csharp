using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FischBot.Handlers
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public InteractionHandler(IConfiguration configuration, DiscordSocketClient discordClient, InteractionService commands, IServiceProvider services, ILogger<InteractionHandler> logger)
        {
            _discordClient = discordClient;
            _commands = commands;
            _services = services;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _discordClient.InteractionCreated += HandleInteraction;

            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;

            _discordClient.Ready += async () =>
            {
                // If running the bot with DEBUG flag, register all commands to guild specified in config
                if (IsDebug())
                {
                    var testGuildId = _configuration.GetValue<ulong>("FischBot:testGuildId");

                    var registeredCommands = await _services
                        .GetRequiredService<InteractionService>()
                        .RegisterCommandsToGuildAsync(testGuildId, true);

                    Console.WriteLine($"Registered the following interaction commands: {string.Join(',', registeredCommands.Select(command => command.Name))}");
                }
                else
                {
                    await _services
                        .GetRequiredService<InteractionService>()
                        .RegisterCommandsGloballyAsync(true);
                }
            };
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            var context = new SocketInteractionContext(_discordClient, interaction);

            try
            {
                await _commands.ExecuteCommandAsync(context, _services);
            }
            catch (Exception)
            {
                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }

        private async Task ComponentCommandExecuted(ComponentCommandInfo command, Discord.IInteractionContext context, IResult result)
        {
            if (result.IsSuccess)
            {
                _logger.LogInformation($"Component command [{command.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
            }
            else
            {
                _logger.LogError($"Component command failed to execute for [{context.User.Username}] <-> [{result}]!");
                await context.Interaction.RespondAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!", ephemeral: true);
            }
        }

        private async Task ContextCommandExecuted(ContextCommandInfo command, Discord.IInteractionContext context, IResult result)
        {
            if (result.IsSuccess)
            {
                _logger.LogInformation($"Context command [{command.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
            }
            else
            {
                _logger.LogError($"Context command failed to execute for [{context.User.Username}] <-> [{result}]!");
                await context.Interaction.RespondAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!", ephemeral: true);
            }
        }

        private async Task SlashCommandExecuted(SlashCommandInfo command, Discord.IInteractionContext context, IResult result)
        {
            if (result.IsSuccess)
            {
                _logger.LogInformation($"Slash command [{command.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
            }
            else
            {
                _logger.LogError($"Slash command failed to execute for [{context.User.Username}] <-> [{result}]!");
                if (context.Interaction.HasResponded)
                    await context.Interaction.RespondAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!", ephemeral: true);
                else
                    await context.Interaction.FollowupAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!", ephemeral: true);
            }
        }

        static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
