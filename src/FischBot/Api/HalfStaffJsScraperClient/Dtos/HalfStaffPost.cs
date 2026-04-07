using System.Text.Json.Serialization;

namespace FischBot.Api.HalfStaffJsScraperClient.Dtos
{
    public class HalfStaffPost
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        [JsonPropertyName("date_start")]
        public string DateStart { get; set; }

        [JsonPropertyName("date_end")]
        public string DateEnd { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; }
    }
}
