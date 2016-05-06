
using Noobot.Examples.ConsoleService.Extensions;

namespace Noobot.Examples.ConsoleService.LuisAi
{
    public class LuisIntent
    {
        public string Intent { get; set; }

        public string Score { get; set; }

        public LuisResponseValue LuisResponse {
            get
            {
                return Intent.ToEnum<LuisResponseValue>();
            }
        }
    }
}
