namespace FischBot.Api.CalendarificHolidaysApiClient.Dtos
{
    public class CalendarificApiResponse<T>
    {
        public Metadata Meta { get; set; }
        public T Response { get; set; }
    }
}