using FischBot.Api.OpenWeatherMapApiClient.Dtos;
using System.Threading.Tasks;

namespace FischBot.Api.OpenWeatherMapApiClient
{
    public interface IOpenWeatherMapApiClient
    {
        Task<OpenWeatherMapApiResponse> GetWeather(string cityName);
        Task<OpenWeatherMapApiResponse> GetWeatherByPostalCode(string postalCode);
    }
}
