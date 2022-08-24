using System;
using System.IO;

namespace FischBot.Api.DeepAiApiClient.Dtos
{
    public class TextToImageResponse
    {
        public string Id { get; set; }
        public string Output_Url { get; set; }
        public Stream ImageStream { get; set; }
    }
}
