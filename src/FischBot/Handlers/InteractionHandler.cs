using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
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

        public InteractionHandler(DiscordSocketClient discordClient, InteractionService commands, IServiceProvider services) 
        {
            _discordClient = discordClient;
            _commands = commands;
            _services = services;
            _logger = services.GetRequiredService<ILogger<InteractionHandler>>();
        }
    
        public async Task InitializeAsync() 
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _discordClient.InteractionCreated += HandleInteraction;

            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
        }

        private async Task HandleInteraction(SocketInteraction interaction) 
        {
            var context = new SocketInteractionContext(_discordClient, interaction);

            try 
            {
                await _commands.ExecuteCommandAsync(context, _services);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Command failed to execute for [{context.User.Username}] <-> [{ex.Message}]!");

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
                _logger.LogError($"Command failed to execute for [{context.User.Username}] <-> [{result}]!");
                await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
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
                _logger.LogError($"Command failed to execute for [{context.User.Username}] <-> [{result}]!");
                await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
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
                _logger.LogError($"Command failed to execute for [{context.User.Username}] <-> [{result}]!");
                await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
            }
        }
    }
}
