using System;

namespace FischBot.Api.NasaApiClient.Dtos
{
    public class ApodResponse
    {
        public DateTime Date { get; set; }
        public string Explanation { get; set; }
        public string HdUrl { get; set; }
        public string Url { get; set; }
        public string Media_Type { get; set; }
        public string Thumbnail_Url { get; set; }
        public string Title { get; set; }
    }
}