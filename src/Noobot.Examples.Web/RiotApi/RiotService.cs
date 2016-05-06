
using System.IO;
using System.Net;

namespace Noobot.Examples.ConsoleService.RiotApi
{
    public class RiotService
    {
        private readonly string _baseUrl = "https://euw.api.pvp.net/api/lol/euw/v2.2/matchlist/by-summoner/";
        private readonly string _indexes = "/?beginIndex=0&endIndex=1";
        private readonly string _apiKey = "&api_key=d3e4d7ea-3625-4e6f-98da-817ce5832735";
        private readonly int _dreagenId = 28147640;

        public string GetLastPlayedChampion(string summoner = "Dreagen")
        {
            string url = string.Empty;
            if (summoner.Equals("Dreagen"))
            {
                url = _baseUrl + _dreagenId + _indexes + _apiKey;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";

            using (var streamReader = new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();

                return result;
            }
        }
    }
}
