using System.Text.Json.Serialization;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class Sys
    {
        [JsonPropertyName("country")]
        public string CountryCode { get; set; }
    }
}
