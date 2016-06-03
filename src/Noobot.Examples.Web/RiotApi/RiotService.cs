using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noobot.Examples.Web.RiotApi.Models;

namespace Noobot.Examples.Web.RiotApi
{
    public class RiotService : IRiotService
    {
        private readonly string _euwBaseUrl = "https://euw.api.pvp.net/api/lol/euw";
        private readonly string _globalBaseUrl = "https://global.api.pvp.net/api/lol/static-data/euw/v1.2";
        private readonly string _apiKey;
        private readonly int _dreagenId = 28147640;
        public const string _riotError = "Having trouble contacting riot... try again in a few mins";

        public RiotService(string apiKey)
        {
            _apiKey = $"api_key={apiKey}";
        }
        public string GetLastPlayedChampion(string summoner = "Dreagen")
        {
            try
            {
                List<Match> lastMatchesForSummonerId = GetLastMatchesForSummonerId(_dreagenId, 1);
                if (!lastMatchesForSummonerId.Any())
                {
                    return null;
                }

                Champion champion = GetChampionById(lastMatchesForSummonerId.First().Champion);

                return $"{champion.Name}, {champion.Title}";
            }
            catch (WebException)
            {
                return _riotError;
            }
        }

        public string GetResultOfLastGame(string summoner = "Dreagen")
        {
            try
            {
                List<Match> lastMatchesForSummonerId = GetLastMatchesForSummonerId(_dreagenId, 1);
                if (!lastMatchesForSummonerId.Any())
                {
                    return null;
                }

                Match lastMatch = lastMatchesForSummonerId.First();
                Champion champion = GetChampionById(lastMatch.Champion);

                Participant participant = GetParticipantStatsForMatch(lastMatch, champion);

                return participant.Stats.Winner ? "win" : "loss";
            }
            catch (WebException)
            {
                return _riotError;
            }
        }

        public string GetStatsOfLastGame(string summoner = "Dreagen")
        {
            try
            {
                List<Match> lastMatchesForSummonerId = GetLastMatchesForSummonerId(_dreagenId, 1);
                if (!lastMatchesForSummonerId.Any())
                {
                    return null;
                }

                Match lastMatch = lastMatchesForSummonerId.First();
                Champion champion = GetChampionById(lastMatch.Champion);

                Participant participant = GetParticipantStatsForMatch(lastMatch, champion);

                string result = participant.Stats.Winner ? "win" : "loss";
                return
                    $"Last match stats: Result: {result} KDA: {participant.Kda}, Kills: {participant.Stats.Kills}, Deaths: {participant.Stats.Deaths}, Assists: {participant.Stats.Assists}";
            }
            catch (WebException)
            {
                return _riotError;
            }
        }

        private Participant GetParticipantStatsForMatch(Match match, Champion champion)
        {
            string url = $"{_euwBaseUrl}/v2.2/match/{match.MatchId}?{_apiKey}";
            string result = GetJsonResponseFromUrl(url);

            var jObject = JObject.Parse(result);
            List<JToken> matchesJToken = jObject["participants"].Children().ToList();

            var participants = new List<Participant>();
            foreach (JToken matchToken in matchesJToken)
            {
                Participant participant = JsonConvert.DeserializeObject<Participant>(matchToken.ToString());
                participants.Add(participant);
            }

            return participants.FirstOrDefault(x => x.ChampionId == champion.Id);
        }

        private List<Match> GetLastMatchesForSummonerId(int summonerId, int numberOfMatches = 1)
        {
            List<Match> matches = new List<Match>();
            string url = $"{_euwBaseUrl}/v2.2/matchlist/by-summoner/{summonerId}/?beginIndex=0&endIndex={numberOfMatches}&{_apiKey}";

            string result = GetJsonResponseFromUrl(url);

            var jObject = JObject.Parse(result);
            List<JToken> matchesJToken = jObject["matches"].Children().ToList();

            foreach (JToken matchToken in matchesJToken)
            {
                Match match = JsonConvert.DeserializeObject<Match>(matchToken.ToString());
                matches.Add(match);
            }

            return matches;
        }

        private Champion GetChampionById(int championId)
        {
            string url = $"{_globalBaseUrl}/champion/{championId}?{_apiKey}";
            string result = GetJsonResponseFromUrl(url);
            Champion champion = JsonConvert.DeserializeObject<Champion>(result);

            return champion;
        }

        private string GetJsonResponseFromUrl(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.ContentType = "application/json";

            var httpWebResponse = (HttpWebResponse) request.GetResponse();

            using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
