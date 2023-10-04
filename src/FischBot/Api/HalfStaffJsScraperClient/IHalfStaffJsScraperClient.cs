using System.Threading.Tasks;
using FischBot.Api.HalfStaffJsScraperClient.Dtos;

namespace FischBot.Api.HalfStaffJsScraperClient
{
    public interface IHalfStaffJsScraperClient
    {
        Task<HalfStaffStatus> GetHalfStaffStatus(string state = "");
    }
}