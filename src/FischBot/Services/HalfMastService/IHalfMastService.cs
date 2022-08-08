using System.Threading.Tasks;
using FischBot.Models;

namespace FischBot.Services.HalfMastService
{
    public interface IHalfMastService
    {
        Task<HalfMastNotice> GetLatestHalfMastNotice(string state);
    }
}