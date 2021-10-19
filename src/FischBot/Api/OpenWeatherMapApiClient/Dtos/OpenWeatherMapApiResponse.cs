using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FischBot.Api.OpenWeatherMapApiClient.Dtos
{
    public class OpenWeatherMapApiResponse
    {
        /// <summary>
        ///     City geo location
        /// </summary>
        [JsonPropertyName("coord")]
        public Coordinates Coordinates { get; set; }

        /// <summary>
        ///     More weather info
        /// </summary>
        [JsonPropertyName("weather")]
        public Weather[] Weather { get; set; }

        /// <summary>
        ///     Used internally by API
        /// </summary>
        //[JsonPropertyName("base")]
        //public string Base { get; set; }

        /// <summary>
        ///     Main weather information
        /// </summary>
        [JsonPropertyName("main")]
        public Main Main { get; set; }

        /// <summary>
        ///     Current visibility
        /// </summary>
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        /// <summary>
        ///     Wind conditions
        /// </summary>
        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }

        /// <summary>
        ///     Cloud cover information
        /// </summary>
        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }

        /// <summary>
        ///     Time of data calculation in UTC
        /// </summary>
        [JsonPropertyName("dt")]
        public long TimeOfDataCalculation { get; set; }

        /// <summary>
        ///     Rain information
        /// </summary>
        [JsonPropertyName("rain")]
        public Precipitation Rain { get; set; }

        /// <summary>
        ///     Snow information
        /// </summary>
        [JsonPropertyName("snow")]
        public Precipitation Snow { get; set; }

        /// <summary>
        /// Additional information
        /// </summary>
        [JsonPropertyName("sys")]
        public Sys Additional { get; set; }

        /// <summary>
        ///     The timezone offset
        /// </summary>
        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        /// <summary>
        ///     The current city's id
        /// </summary>
        [JsonPropertyName("id")]
        public long CityId { get; set; }

        /// <summary>
        ///     The current city's name
        /// </summary>
        [JsonPropertyName("name")]
        public string CityName { get; set; }

        /// <summary>
        /// Used internally by API
        /// </summary>
        //[JsonPropertyName("cod")]
        //public string Cod { get; set; }
    }
}
