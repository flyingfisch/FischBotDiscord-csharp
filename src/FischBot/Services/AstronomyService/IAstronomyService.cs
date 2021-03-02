using System;
using System.Threading.Tasks;
using FischBot.Models.Astronomy;

namespace FischBot.Services.AstronomyService
{
    public interface IAstronomyService
    {
        Task<PictureOfTheDay> GetPictureOfTheDay(DateTime date);
    }
}