namespace Noobot.Examples.Web.RiotApi
{
    public interface IRiotService
    {
        string GetLastPlayedChampion(string summoner = "Dreagen");
        string GetResultOfLastGame(string summoner = "Dreagen");
        string GetStatsOfLastGame(string summoner = "Dreagen");
    }
}