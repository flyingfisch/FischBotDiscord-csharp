using System.Threading.Tasks;
using FischBot.Api.CalendarificHolidaysApiClient.Dtos;

namespace FischBot.Api.CalendarificHolidaysApiClient
{
    public interface ICalendarificHolidaysApiClient
    {
        Task<CalendarificApiResponse<CalendarificHolidaysApiResponse>> GetHolidays(string countryCode, int year, int? month = null, int? day = null);
    }
}