using System;
using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Api.NasaApiClient.Dtos;
using Microsoft.Extensions.Configuration;

namespace FischBot.Api.NasaApiClient
{
    public class NasaApiClient : INasaApiClient
    {
        private const string _nasaApiBaseUrl = "https://api.nasa.gov/";
        private readonly HttpClient _httpClient;
        private readonly string _nasaApiKey;

        public NasaApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _nasaApiKey = configuration.GetSection("FischBot:nasaApiKey").Value;
        }

        public async Task<ApodResponse> GetAstronomyPictureOfTheDay(DateTime date)
        {
            var response = await _httpClient.GetAsync($"{_nasaApiBaseUrl}planetary/apod?api_key={_nasaApiKey}&date={date.ToString("yyyy-MM-dd")}");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ApodResponse>();
        }
    }
}