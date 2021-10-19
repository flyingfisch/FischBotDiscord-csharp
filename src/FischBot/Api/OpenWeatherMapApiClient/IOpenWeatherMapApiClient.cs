using FischBot.Api.OpenWeatherMapApiClient.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FischBot.Api.OpenWeatherMapApiClient
{
    public interface IOpenWeatherMapApiClient
    {
        Task<OpenWeatherMapApiResponse> GetWeather(string cityName);
    }
}
