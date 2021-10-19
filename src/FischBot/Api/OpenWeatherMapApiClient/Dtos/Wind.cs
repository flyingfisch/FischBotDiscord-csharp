using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class Wind
    {
        [JsonPropertyName("speed")]
        public float Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Degrees { get; set; }
    }
}
