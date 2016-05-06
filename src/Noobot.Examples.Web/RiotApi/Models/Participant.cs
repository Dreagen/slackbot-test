namespace Noobot.Examples.Web.RiotApi.Models
{
    public class Participant
    {
        public Stats Stats { get; set; }

        public int ChampionId { get; set; }

        public double Kda => (Stats.Kills + Stats.Assists)/(double)Stats.Deaths;
    }
}