using System;
using System.IO;
using System.Threading.Tasks;
using FischBot.Api.DeepAiApiClient;
using FischBot.Models.ArtificialIntelligence;
using Microsoft.Extensions.Configuration;

namespace FischBot.Services.ArtificialIntelligenceService
{
    public class ArtificialIntelligenceService : IArtificialIntelligenceService
    {
        private IDeepAiApiClient _deepAiClient;

        public ArtificialIntelligenceService(IConfiguration configuration, IDeepAiApiClient deepAiApiClient) 
        {
            _deepAiClient = deepAiApiClient;
        }

        public async Task<TextToImageData> GetImageFromText(string text)
        {
            var imageFromText = await _deepAiClient.GetImageFromText(text);

            return new TextToImageData() { ImageUrl = imageFromText.Output_Url };
        }
    }
}
