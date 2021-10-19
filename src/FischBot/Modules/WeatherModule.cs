using Discord;
using Discord.Commands;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.WeatherService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FischBot.Modules
{
    public class WeatherModule : FischBotModuleBase<SocketCommandContext>
    {
        private readonly IWeatherService _weatherService;

        public WeatherModule(IDiscordModuleService moduleService, IWeatherService weatherService) : base(moduleService)
        {
            _weatherService = weatherService;
        }

        [Command("weather")]
        [Summary("Gets the weather for a specified city.")]
        public async Task GetWeatherForCity([Summary("The city name. (e.g. London, \"New York\")")] string cityName)
        {
            try
            {
                var currentWeather = await _weatherService.GetCurrentWeather(cityName);

                var embed = new EmbedBuilder()
                    .WithTitle(currentWeather.Location)
                    .WithDescription(currentWeather.Weather)
                    .WithThumbnailUrl(currentWeather.WeatherIconUrl)
                    .WithFields(
                        new EmbedFieldBuilder().WithName("Temperature").WithValue(currentWeather.TemperatureFormatted),
                        new EmbedFieldBuilder().WithName("Humidity").WithValue(currentWeather.HumidityFormatted)
                    )
                    .WithColor(0, 255, 255)
                    .Build();

                await ReplyAsync(embed: embed);
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        await ReplyAsync("Sorry, the city you are looking for wasn't found.\n(tip: try surrounding the city name in quotes like this \"New York\")");
                        break;
                }

            }
        }
    }
}
