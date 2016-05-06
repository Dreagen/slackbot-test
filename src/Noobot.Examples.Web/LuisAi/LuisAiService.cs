using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Noobot.Examples.Web.LuisAi
{
    class LuisAiService
    {
        private readonly string _baseUrl = "https://api.projectoxford.ai/luis/v1/application?id=028eccf8-1c71-48cf-b292-83c92e17134f&subscription-key=7af2211d904c4a86bb42c744a157cc72&q=";

        public LuisIntent GetResponseFromMessage(string message)
        {
            string url = _baseUrl + message;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";

            using (var streamReader = new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                LuisResponse responseModel = JsonConvert.DeserializeObject<LuisResponse>(result);

                return responseModel.GetMostAccurateIntent();
            }
        }
    }

    public enum LuisResponseValue
    {
        LastPlayedChampion,
        None
    }
}
