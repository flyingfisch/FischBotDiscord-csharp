namespace FischBot.Api.StatsDbClient.Dtos.RainbowSix
{
    public class GetUserStatsPayload
    {
        public User User { get; set; }
        public UserStatistics Stats { get; set; }
    }
}