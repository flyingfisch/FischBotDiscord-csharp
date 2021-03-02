using System;
using System.Threading.Tasks;
using FischBot.Api.NasaApiClient.Dtos;

namespace FischBot.Api.NasaApiClient
{
    public interface INasaApiClient
    {
        Task<ApodResponse> GetAstronomyPictureOfTheDay(DateTime date);
    }
}