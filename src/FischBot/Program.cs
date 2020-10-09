using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FischBot.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FischBot
{
    public class Program
    {
        private readonly IConfiguration _configuration;
        private DiscordSocketClient _discordClient;

        public Program()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }


        public static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            using (var services = ConfigureServices())
            {
                var discordClient = services.GetRequiredService<DiscordSocketClient>();
                _discordClient = discordClient;

                _discordClient.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                await _discordClient.LoginAsync(TokenType.Bot, _configuration.GetSection("FischBot:token").Value);
                await _discordClient.StartAsync();

                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private DiscordSocketClient BuildDiscordSocketClient(IConfiguration configuration)
        {
            var config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 50,
            };

            return new DiscordSocketClient(config);
        }


        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .AddSingleton<HtmlWeb>()
                .AddTransient<UsFlagHalfMastService>();

            return services.BuildServiceProvider();
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());

            return Task.CompletedTask;
        }
    }
}
