using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FischBot.Services
{
    public class LoggingService
    {
        private readonly ILogger _logger;
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;

        public LoggingService(IServiceProvider services)
        {
            _discordClient = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _logger = services.GetRequiredService<ILogger<LoggingService>>();

            _discordClient.Ready += OnReadyAsync;

            _discordClient.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }

        public Task OnReadyAsync()
        {
            _logger.LogInformation($"Connected as -> [{_discordClient.CurrentUser}] :)");
            _logger.LogInformation($"Connected to [{_discordClient.Guilds.Count}] servers");

            return Task.CompletedTask;
        }

        private Task OnLogAsync(LogMessage message)
        {
            string logText = $"{message.Source}: {message.Exception?.ToString() ?? message.Message}";

            switch (message.Severity.ToString())
            {
                case "Critical":
                    _logger.LogCritical(logText);
                    break;
                case "Warning":
                    _logger.LogWarning(logText);
                    break;
                case "Info":
                    _logger.LogInformation(logText);
                    break;
                case "Verbose":
                    _logger.LogInformation(logText);
                    break;
                case "Debug":
                    _logger.LogDebug(logText);
                    break;
                case "Error":
                    _logger.LogError(logText);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}