using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FischBot.Api.HalfStaffJsScraperClient.Dtos;

namespace FischBot.Api.HalfStaffJsScraperClient
{
    public class HalfStaffJsScraperClient : IHalfStaffJsScraperClient
    {
        private const string _halfStaffScriptUrl = "https://halfstaff.org/widgets/us-half-staff-flags.js";
        private const string _halfStaffReasonRegexPattern = @"(?<=var dataevent =')(.*)(?=')";
        private readonly HttpClient _httpClient;

        public HalfStaffJsScraperClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the US flag's half staff status by scraping the javascript file that powers the halfstaff.org widget.
        /// </summary>
        public async Task<HalfStaffStatus> GetHalfStaffStatus(string state = "")
        {
            var requestUrl = string.IsNullOrEmpty(state) ? _halfStaffScriptUrl : $"{_halfStaffScriptUrl}?st={state}";
            var response = await _httpClient.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // the first line of the JS file will look like this if the flag is at half staff: 
            // var dataevent ='Half Staff in Honor of Dianne Feinstein';
            // so we'll use that information to determine whether the flag is at half staff.
            var halfStaffRegexResult = Regex.Match(responseString, _halfStaffReasonRegexPattern);
            var halfStaffReason = halfStaffRegexResult.Groups[0].Value;
            var isHalfStaff = !string.IsNullOrEmpty(halfStaffReason);

            return new HalfStaffStatus()
            {
                IsHalfStaff = isHalfStaff,
                Reason = halfStaffReason,
            };
        }
    }
}