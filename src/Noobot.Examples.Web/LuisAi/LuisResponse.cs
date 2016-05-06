using System.Collections.Generic;

namespace Noobot.Examples.Web.LuisAi
{
    public class LuisResponse
    {
        public string Query { get; set; }

        public List<LuisIntent> Intents { get; set; }

        public LuisIntent GetMostAccurateIntent()
        {
            LuisIntent result = null;
            float maxScore = float.MinValue;

            foreach (LuisIntent intent in Intents)
            {
                float intentScore = float.Parse(intent.Score);
                if (intentScore > maxScore)
                {
                    maxScore = intentScore;
                    result = intent;
                }
            }

            return result;
        }
    }
}
