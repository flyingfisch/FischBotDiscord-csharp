using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class Clouds
    {
        /// <summary>
        ///     Cloudiness percentage
        /// </summary>
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
