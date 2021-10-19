using FischBot.Api.OpenWeatherMapApiClient.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FischBot.Api.OpenWeatherMapApiClient
{
    public class OpenWeatherMapApiClient : IOpenWeatherMapApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenWeatherMapApiClient> _logger;

        private readonly string _openWeatherMapBaseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private readonly string _openWeatherMapApiKey;

        public OpenWeatherMapApiClient(HttpClient httpClient, IConfiguration configuration, ILogger<OpenWeatherMapApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _openWeatherMapApiKey = configuration.GetSection("FischBot:weatherApiKey").Value;
        }

        public async Task<OpenWeatherMapApiResponse> GetWeather(string cityName)
        {
            using var _ = _logger.BeginScope("Getting weather");

            var queryParams = new Dictionary<string, string>
            {
                ["appId"] = _openWeatherMapApiKey
            };

            if (!string.IsNullOrWhiteSpace(cityName))
                queryParams["q"] = cityName;
            else
                throw new ArgumentNullException(nameof(cityName));

            var url = QueryHelpers.AddQueryString(_openWeatherMapBaseUrl, queryParams);

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var des = JsonSerializer.Deserialize<OpenWeatherMapApiResponse>(json);

            return JsonSerializer.Deserialize<OpenWeatherMapApiResponse>(json);
        }
    }
}
