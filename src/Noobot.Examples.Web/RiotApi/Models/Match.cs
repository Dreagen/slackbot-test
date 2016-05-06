namespace Noobot.Examples.Web.RiotApi.Models
{
    public class Match
    {
        public string Region { get; set; }

        public string PlatformId { get; set; }

        public long MatchId { get; set; }

        public int Champion { get; set; }

        public string Queue { get; set; }

        public string Season { get; set; }

        public string Timestamp { get; set; }

        public string Lane { get; set; }

        public string Role { get; set; }
    }
}