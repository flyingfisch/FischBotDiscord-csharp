using System;
using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Api.HalfStaffJsScraperClient.Dtos;

namespace FischBot.Api.HalfStaffJsScraperClient
{
    public class HalfStaffJsScraperClient : IHalfStaffJsScraperClient
    {
        private const string _currentNoticesUrl = "https://halfstaff.org/";
        private const string _fallbackSourceUrl = "https://halfstaff.org/";
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);
        private readonly HttpClient _httpClient;
        private readonly HalfStaffPageParser _halfStaffPageParser;
        private readonly object _cacheLock = new object();
        private HalfStaffStatus _cachedStatus;
        private DateTimeOffset _cacheExpiresAt;

        public HalfStaffJsScraperClient(HttpClient httpClient, HalfStaffPageParser halfStaffPageParser)
        {
            _httpClient = httpClient;
            _halfStaffPageParser = halfStaffPageParser;
        }

        /// <summary>
        /// Gets the current half-staff status from the active notices section on halfstaff.org.
        /// </summary>
        public async Task<HalfStaffStatus> GetHalfStaffStatus()
        {
            var currentTime = DateTimeOffset.UtcNow;
            lock (_cacheLock)
            {
                if (_cachedStatus != null && currentTime < _cacheExpiresAt)
                {
                    return _cachedStatus;
                }
            }

            try
            {
                var response = await _httpClient.GetAsync(_currentNoticesUrl);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var halfStaffStatus = _halfStaffPageParser.ParseCurrentStatus(responseString, currentTime, _fallbackSourceUrl);

                lock (_cacheLock)
                {
                    _cachedStatus = halfStaffStatus;
                    _cacheExpiresAt = currentTime.Add(_cacheDuration);
                }

                return halfStaffStatus;
            }
            catch (Exception)
            {
                return new HalfStaffStatus
                {
                    IsStatusKnown = false,
                    SourceUrl = _fallbackSourceUrl,
                };
            }
        }
    }
}
