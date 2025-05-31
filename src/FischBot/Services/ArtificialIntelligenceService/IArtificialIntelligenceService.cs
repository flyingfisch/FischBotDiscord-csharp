using System.Threading.Tasks;
using FischBot.Models.ArtificialIntelligence;

namespace FischBot.Services.ArtificialIntelligenceService
{
    public interface IArtificialIntelligenceService
    {
        Task<TextToImageData> GetImageFromText(string text);
    }
}
