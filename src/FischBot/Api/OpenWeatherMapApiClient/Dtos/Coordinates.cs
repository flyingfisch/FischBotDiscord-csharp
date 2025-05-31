using System.Text.Json.Serialization;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class Coordinates
    {
        /// <summary>
        ///     City longitude
        /// </summary>
        [JsonPropertyName("lon")]
        public float Longitude { get; set; }

        /// <summary>
        ///     City latitude
        /// </summary>
        [JsonPropertyName("lat")]
        public float Latitude { get; set; }
    }
}
