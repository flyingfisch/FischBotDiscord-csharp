namespace FischBot.Api.StatsDbClient.Dtos.RainbowSix
{
    public class RankedStatistics
    {
        public int Rank { get; set; }
        public int Mmr { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Abandons { get; set; }
    }
}