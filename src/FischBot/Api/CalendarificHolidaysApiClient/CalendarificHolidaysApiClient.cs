using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Api.CalendarificHolidaysApiClient.Dtos;
using Microsoft.Extensions.Configuration;

namespace FischBot.Api.CalendarificHolidaysApiClient
{
    public class CalendarificHolidaysApiClient : ICalendarificHolidaysApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string _holidaysApiBaseUrl = "https://calendarific.com/api/v2/";
        private readonly string _holidaysApiKey;

        public CalendarificHolidaysApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _holidaysApiKey = configuration.GetSection("FischBot:holidaysApiKey").Value;
        }

        public async Task<CalendarificApiResponse<CalendarificHolidaysApiResponse>> GetHolidays(string countryCode, int year, int? month = null, int? day = null)
        {
            var response = await _httpClient.GetAsync($"{_holidaysApiBaseUrl}holidays?api_key={_holidaysApiKey}&country={countryCode}&year={year}&month={month}&day={day}");

            response.EnsureSuccessStatusCode();
            var deserializedResponse = await response.Content.ReadAsAsync<CalendarificApiResponse<CalendarificHolidaysApiResponse>>();

            return deserializedResponse;
        }
    }
}