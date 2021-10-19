using FischBot.Api.OpenWeatherMapApiClient;
using FischBot.Models.Weather;
using System;
using System.Threading.Tasks;

namespace FischBot.Services.WeatherService
{
    public class WeatherService : IWeatherService
    {
        private readonly IOpenWeatherMapApiClient _weatherApiClient;

        public WeatherService(IOpenWeatherMapApiClient weatherApiClient)
        {
            _weatherApiClient = weatherApiClient;
        }

        public async Task<CurrentWeather> GetCurrentWeather(string cityName)
        {

            var response = await _weatherApiClient.GetWeather(cityName);

            static string GetWeatherIconUrl(string icon)
                => $"http://openweathermap.org/img/wn/{icon}@4x.png";

            var currentWeather = new CurrentWeather
            {
                CityName = response.CityName,
                CountryCode = response.Additional.CountryCode,
                WeatherIconUrl = GetWeatherIconUrl(response.Weather[0].Icon),
                Weather = response.Weather[0].Main,
                WeatherDescription = response.Weather[0].Description,
                DateCalculated = new DateTime(response.TimeOfDataCalculation),
                CurrentTemperature = response.Main.Temperature,
                Humidity = response.Main.Humidity
            };

            return currentWeather;
        }

        public async Task<CurrentWeather> GetCurrentWeatherByPostalCode(string postalCode)
        {

            var response = await _weatherApiClient.GetWeatherByPostalCode(postalCode);

            static string GetWeatherIconUrl(string icon)
                => $"http://openweathermap.org/img/wn/{icon}@4x.png";

            var currentWeather = new CurrentWeather
            {
                CityName = response.CityName,
                CountryCode = response.Additional.CountryCode,
                WeatherIconUrl = GetWeatherIconUrl(response.Weather[0].Icon),
                Weather = response.Weather[0].Main,
                WeatherDescription = response.Weather[0].Description,
                DateCalculated = new DateTime(response.TimeOfDataCalculation),
                CurrentTemperature = response.Main.Temperature,
                Humidity = response.Main.Humidity
            };

            return currentWeather;
        }
    }
}
