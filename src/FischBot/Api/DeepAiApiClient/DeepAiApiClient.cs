using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FischBot.Api.DeepAiApiClient.Dtos;
using Microsoft.Extensions.Configuration;

namespace FischBot.Api.DeepAiApiClient
{
    public class DeepAiApiClient : IDeepAiApiClient
    {
        private readonly string _deepAiBaseUrl = "https://api.deepai.org/";
        private readonly HttpClient _httpClient;
        private readonly string _deepAiApiKey;

        public DeepAiApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _deepAiApiKey = configuration.GetSection("FischBot:deepAiApiKey").Value;
        }

        public async Task<TextToImageResponse> GetImageFromText(string text)
        {
            var imageData = await GenerateImage(text);
            imageData.ImageStream = await DownloadImage(imageData.Output_Url);

            return imageData;
        }

        private async Task<TextToImageResponse> GenerateImage(string text) 
        {
            var requestData = new Dictionary<string, string>() 
            {
                { "text", text }
            };

            var requestContent = new FormUrlEncodedContent(requestData);
            requestContent.Headers.Add("api-key", _deepAiApiKey);

            var response = await _httpClient.PostAsync($"{_deepAiBaseUrl}api/text2img", requestContent);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<TextToImageResponse>();
        }

        private async Task<Stream> DownloadImage(string imageUrl)
        {
            var image = await _httpClient.GetAsync(imageUrl);

            return await image.Content.ReadAsStreamAsync();
        }
    }
}
