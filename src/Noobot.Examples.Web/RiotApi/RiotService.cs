using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noobot.Examples.Web.RiotApi.Models;

namespace Noobot.Examples.Web.RiotApi
{
    public class RiotService
    {
        private readonly string _euwBaseUrl = "https://euw.api.pvp.net/api/lol/euw";
        private readonly string _globalBaseUrl = "https://global.api.pvp.net/api/lol/static-data/euw/v1.2";
        private readonly string _apiKey = "api_key=d3e4d7ea-3625-4e6f-98da-817ce5832735";
        private readonly int _dreagenId = 28147640;

        public string GetLastPlayedChampion(string summoner = "Dreagen")
        {
            List<Match> lastMatchesForSummonerId = GetLastMatchesForSummonerId(_dreagenId, 1);
            if (!lastMatchesForSummonerId.Any())
            {
                return null;
            }

            Champion champion =  GetChampionById(lastMatchesForSummonerId.First().Champion);

            return $"{champion.Name}, {champion.Title}";
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
