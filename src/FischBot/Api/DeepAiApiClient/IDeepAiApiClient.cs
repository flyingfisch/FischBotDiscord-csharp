using System.Threading.Tasks;
using FischBot.Api.DeepAiApiClient.Dtos;

namespace FischBot.Api.DeepAiApiClient
{
    public interface IDeepAiApiClient
    {
        Task<TextToImageResponse> GetImageFromText(string text);
    }
}
