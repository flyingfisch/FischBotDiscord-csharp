using System.Threading.Tasks;
using FischBot.Api.HalfStaffJsScraperClient;
using FischBot.Models;
using Microsoft.Extensions.Configuration;

namespace FischBot.Services.HalfMastService
{
    public class HalfMastService : IHalfMastService
    {
        private readonly IConfiguration _configuration;
        private readonly IHalfStaffJsScraperClient _halfStaffJsScraperClient;

        public HalfMastService(IConfiguration configuration, IHalfStaffJsScraperClient halfStaffJsScraperClient)
        {
            _configuration = configuration;
            _halfStaffJsScraperClient = halfStaffJsScraperClient;
        }

        public async Task<HalfMastStatus> GetHalfMastStatus(string state)
        {
            var response = await _halfStaffJsScraperClient.GetHalfStaffStatus(state);

            var halfMastStatus = new HalfMastStatus()
            {
                State = state?.ToUpper(),
                IsHalfStaff = response.IsHalfStaff,
                Description = response.Reason,
                SourceUrl = $"https://halfstaff.org/"
            };

            return halfMastStatus;
        }
    }
}
