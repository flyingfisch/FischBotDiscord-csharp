using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using FischBot.Api.CalendarificHolidaysApiClient;
using FischBot.Api.DeepAiApiClient;
using FischBot.Api.HalfStaffJsScraperClient;
using FischBot.Api.NasaApiClient;
using FischBot.Api.OpenWeatherMapApiClient;
using FischBot.Handlers;
using FischBot.Services;
using FischBot.Services.ArtificialIntelligenceService;
using FischBot.Services.AstronomyService;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.FinanceService;
using FischBot.Services.HalfMastService;
using FischBot.Services.HolidayService;
using FischBot.Services.ImageChartService;
using FischBot.Services.WeatherService;
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

                await services.GetRequiredService<InteractionHandler>().InitializeAsync();

                await _discordClient.SetGameAsync("Type / to see commands");
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
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .AddSingleton<HttpClient>()
                .AddSingleton<HtmlWeb>()
                .AddSingleton<IDiscordModuleService, DiscordModuleService>()
                .AddSingleton<ICalendarificHolidaysApiClient, CalendarificHolidaysApiClient>()
                .AddSingleton<IOpenWeatherMapApiClient, OpenWeatherMapApiClient>()
                .AddSingleton<IHalfStaffJsScraperClient, HalfStaffJsScraperClient>()
                .AddSingleton<IHalfMastService, HalfMastService>()
                .AddSingleton<IHolidayService, HolidayService>()
                .AddSingleton<IFinanceService, FinanceService>()
                .AddSingleton<IImageChartService, ImageChartService>()
                .AddSingleton<INasaApiClient, NasaApiClient>()
                .AddSingleton<IAstronomyService, AstronomyService>()
                .AddSingleton<IWeatherService, WeatherService>()
                .AddSingleton<IDeepAiApiClient, DeepAiApiClient>()
                .AddSingleton<IArtificialIntelligenceService, ArtificialIntelligenceService>()
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
