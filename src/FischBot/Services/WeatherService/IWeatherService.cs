using FischBot.Models.Weather;
using System.Threading.Tasks;

namespace FischBot.Services.WeatherService
{
    public interface IWeatherService
    {
        Task<CurrentWeather> GetCurrentWeather(string cityName);
        Task<CurrentWeather> GetCurrentWeatherByPostalCode(string postalCode);
    }
}
