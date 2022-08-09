using System;

namespace FischBot.Models.Astronomy
{
    public class PictureOfTheDay
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string MediaType { get; set; }

        public DateTime Date { get; set; }
    }
}