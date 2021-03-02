using System;
using System.Threading.Tasks;
using FischBot.Api.NasaApiClient;
using FischBot.Models.Astronomy;

namespace FischBot.Services.AstronomyService
{
    public class AstronomyService : IAstronomyService
    {
        private readonly INasaApiClient _nasaApiClient;

        public AstronomyService(INasaApiClient nasaApiClient)
        {
            _nasaApiClient = nasaApiClient;
        }

        public async Task<PictureOfTheDay> GetPictureOfTheDay(DateTime date)
        {
            var response = await _nasaApiClient.GetAstronomyPictureOfTheDay(date);

            var pictureOfTheDay = new PictureOfTheDay()
            {
                Name = response.Title,
                Caption = response.Explanation,
                Url = response.Url,
                Date = response.Date
            };

            return pictureOfTheDay;
        }
    }
}