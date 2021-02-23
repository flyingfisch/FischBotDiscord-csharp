namespace FischBot.Api.CalendarificHolidaysApiClient.Dtos
{
    public class Holiday
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public CalendarificDate Date { get; set; }
        public string[] Type { get; set; }
        public string Locations { get; set; }
    }
}