using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FischBot.Api.CalendarificHolidaysApiClient;
using FischBot.Models;

namespace FischBot.Services.HolidayService
{
    public class HolidayService : IHolidayService
    {
        private readonly ICalendarificHolidaysApiClient _calendarificHolidaysApiClient;

        public HolidayService(ICalendarificHolidaysApiClient calendarificHolidaysApiClient)
        {
            _calendarificHolidaysApiClient = calendarificHolidaysApiClient;
        }

        public async Task<Holiday> GetHolidayByName(string holidayName, string country, int year)
        {
            var holidayNameRegex = new Regex(@"[^\w\s]");

            var getHolidaysResponse = await _calendarificHolidaysApiClient.GetHolidays(country, year);

            var holidayResponse = getHolidaysResponse.Response.Holidays
                .Where(holiday => holidayNameRegex.Replace(holiday.Name, "").ToLower().Contains(
                    holidayNameRegex.Replace(holidayName, "").ToLower()
                ))
                .OrderBy(holiday => holiday.Name.Length)
                .FirstOrDefault();

            if (holidayResponse == null)
            {
                throw new Exception("Could not find that holiday.");
            }

            var holiday = MapCalendarificHoliday(holidayResponse);

            return holiday;
        }

        public async Task<Holiday[]> GetHolidaysByDate(DateTime date, string country)
        {
            var getHolidaysResponse = await _calendarificHolidaysApiClient.GetHolidays(country, date.Year, date.Month, date.Day);

            var holidays = getHolidaysResponse.Response.Holidays
                .Select(holiday => MapCalendarificHoliday(holiday));

            return holidays.ToArray();
        }

        private Holiday MapCalendarificHoliday(Api.CalendarificHolidaysApiClient.Dtos.Holiday calendarificHoliday)
        {
            var holidayDate = DateTime.Parse(calendarificHoliday.Date.Iso);

            var holiday = new Holiday()
            {
                Country = calendarificHoliday.Country.Id,
                Date = holidayDate,
                Type = string.Join(", ", calendarificHoliday.Type),
                Name = calendarificHoliday.Name
            };

            return holiday;
        }
    }
}