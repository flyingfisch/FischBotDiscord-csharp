namespace FischBot.Api.StatsDbClient.Dtos
{
    public class StatsDbApiResponse<T>
    {
        public T Payload { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Error { get; set; }
        public int Count { get; set; }
    }
}