using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FischBot.Models.Weather
{
    public class CurrentWeather
    {
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string Location => $"{CityName}, {CountryCode}";
        public string WeatherIconUrl { get; set;  }
        public string Weather { get; set; }
        public string WeatherDescription { get; set; }
        public DateTimeOffset DateCalculated { get; set; }
        public float CurrentTemperature { get; set; }

        public int TemperatureFahrenheit => (int)((CurrentTemperature - 273.15) * 9 / 5 + 32);
        public int TemperatureCelsius => (int)(CurrentTemperature - 273.15);

        public string TemperatureFormatted => $"{TemperatureCelsius}℃ / {TemperatureFahrenheit}℉";

        public int Humidity { get; set; }
        public string HumidityFormatted => $"{Humidity}%";
    }
}
