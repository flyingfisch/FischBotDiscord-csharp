using Discord;
using Discord.Interactions;
using FischBot.Services.DiscordModuleService;
using FischBot.Services.WeatherService;
using System.Net.Http;
using System.Threading.Tasks;

namespace FischBot.Modules
{
    [Group("weather", "Weather commands")]
    public class WeatherModule : FischBotInteractionModuleBase<SocketInteractionContext>
    {
        private readonly IWeatherService _weatherService;

        public WeatherModule(IDiscordModuleService moduleService, IWeatherService weatherService) : base(moduleService)
        {
            _weatherService = weatherService;
        }

        [SlashCommand("city", "Gets the weather for a specified city.")]
        public async Task GetWeatherForCity([Summary(description: "The city name. (e.g. London, \"New York\")")] string cityName)
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

                await RespondAsync(embed: embed);
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        await RespondAsync("Sorry, the city you are looking for wasn't found.\n(tip: try surrounding the city name in quotes like this: \"New York\")", ephemeral: true);
                        break;
                }
            }
        }

        [SlashCommand("zip", "Gets the weather for a specified postal code.")]
        public async Task GetWeatherForPostalCode([Summary(description: "The postal code. (e.g., 45014)")] string postalCode)
        {
            try
            {
                var currentWeather = await _weatherService.GetCurrentWeatherByPostalCode(postalCode);

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

                await RespondAsync(embed: embed);
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        await RespondAsync("Sorry, the city you are looking for wasn't found.", ephemeral: true);
                        break;
                }
            }
        }
    }
}
