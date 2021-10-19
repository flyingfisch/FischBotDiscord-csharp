using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class Main
    {
        /// <summary>
        ///     Temperature in Kelvin
        /// </summary>
        [JsonPropertyName("temp")]
        public float Temperature { get; set; }

        /// <summary>
        ///     Temperature based on human perception in Kelvin
        /// </summary>
        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }

        /// <summary>
        ///     Atmospheric pressure in hPa (hectopascal)
        /// </summary>
        [JsonPropertyName("pressure")]
        public float Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        /// <summary>
        ///     Minimum temperature at the moment
        /// </summary>
        [JsonPropertyName("temp_min")]
        public float TemperatureMin { get; set; }

        /// <summary>
        ///     Maximum termperature at the moment
        /// </summary>
        [JsonPropertyName("temp_max")]
        public float TemperatureMax { get; set; }

        /// <summary>
        ///     Atmospheric pressure on the sea level in hPa (hectopascal)
        /// </summary>
        [JsonPropertyName("sea_level")]
        public float SeaLevelPressure { get; set; }

        /// <summary>
        ///     Atmospheric pressure on the ground level in hPa (hectopascal)
        /// </summary>
        [JsonPropertyName("grnd_level")]
        public float GroundLevelPressure { get; set; }
    }
}
