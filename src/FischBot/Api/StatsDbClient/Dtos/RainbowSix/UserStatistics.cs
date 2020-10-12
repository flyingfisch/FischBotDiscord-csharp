namespace FischBot.Api.StatsDbClient.Dtos.RainbowSix
{
    public class UserStatistics
    {
        public RankedStatistics Rank { get; set; }
        public UnrankedStatistics Unranked { get; set; }
        public CasualStatistics Casual { get; set; }
        public GeneralStatistics General { get; set; }
    }
}