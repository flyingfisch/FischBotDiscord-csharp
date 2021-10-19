using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class Percipitation
    {
        /// <summary>
        ///     Volume for the last 1 hour in millimeters
        /// </summary>
        [JsonPropertyName("1h")]
        public float OneHourVolume { get; set; }

        /// <summary>
        ///     Volume for the last 3 hours in millimeters
        /// </summary>
        [JsonPropertyName("3h")]
        public float ThreeHourVolume { get; set; }
    }
}
