namespace FischBot.Api.HalfStaffJsScraperClient.Dtos
{
    public class HalfStaffStatus
    {
        public bool IsStatusKnown { get; set; }
        public bool IsHalfStaff { get; set; }
        public HalfStaffPost[] ActivePosts { get; set; }
        public string ReasonTitle { get; set; }
        public string Reason { get; set; }
        public string SourceUrl { get; set; }
    }
}
