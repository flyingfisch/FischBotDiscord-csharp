using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FischBot.Api.CalendarificHolidaysApiClient;
using FischBot.Api.StarsAndStripesDailyClient;
using FischBot.Api.StatsDbClient;
using FischBot.Handlers;
using FischBot.Services;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.EightBallService;
using FischBot.Services.HolidayService;
using FischBot.Services.RainbowSixService;
using FischBot.Services.RouletteService;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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

            _discordClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 50,
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }


        public static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            using (var services = ConfigureServices())
            {
                services.GetRequiredService<LoggingService>();

                await _discordClient.SetGameAsync($"{_configuration.GetSection("FischBot:commandPrefix").Value}help");
                await _discordClient.LoginAsync(TokenType.Bot, _configuration.GetSection("FischBot:token").Value);
                await _discordClient.StartAsync();

                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton(_discordClient)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .AddSingleton<HtmlWeb>()
                .AddSingleton<IDiscordModuleService, DiscordModuleService>()
                .AddSingleton<IStarsAndStripesDailyClient, StarsAndStripesDailyClient>()
                .AddSingleton<IStatsDbClient, StatsDbClient>()
                .AddSingleton<ICalendarificHolidaysApiClient, CalendarificHolidaysApiClient>()
                .AddSingleton<IRainbowSixService, RainbowSixService>()
                .AddSingleton<IHolidayService, HolidayService>()
                .AddSingleton<IEightBallService, EightBallService>()
                .AddSingleton<IRouletteService, RouletteService>()
                .AddSingleton<LoggingService>()
                .AddLogging(configure => configure.AddSerilog());

            return services.BuildServiceProvider();
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.ToString());

            return Task.CompletedTask;
        }
    }
}
