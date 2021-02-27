using System;
using System.Threading.Tasks;
using FischBot.Models;

namespace FischBot.Services.HolidayService
{
    public interface IHolidayService
    {
        Task<Holiday> GetHolidayByName(string holidayName, string country, int year);
        Task<Holiday[]> GetHolidaysByDate(DateTime date, string country);
    }
}