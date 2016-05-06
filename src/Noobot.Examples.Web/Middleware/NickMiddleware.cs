using System.Collections.Generic;
using System.Configuration;
using Noobot.Core.Configuration;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using Noobot.Core.Plugins.StandardPlugins;
using Noobot.Examples.Web.LuisAi;
using Noobot.Examples.Web.RiotApi;

namespace Noobot.Examples.Web.Middleware
{
    public class NickMiddleware : MiddlewareBase
    {
        private readonly LuisAiService _luisAiService;
        private readonly IRiotService _riotService;

        public NickMiddleware(IMiddleware next) : base(next)
        {
            _luisAiService = new LuisAiService();
            _riotService = new RiotService(ConfigurationManager.AppSettings["riotApiToken"]);
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = new [] { "" },
                    Description = "Ask questions to the riot api",
                    EvaluatorFunc = RiotApiHandler,
                    MessageShouldTargetBot = true
                }
            };
        }

        private IEnumerable<ResponseMessage> RiotApiHandler(IncomingMessage message, string matchedHandle)
        {
            yield return message.IndicateTypingOnChannel();

            LuisIntent result = _luisAiService.GetResponseFromMessage(message.FullText);

            switch (result.LuisResponse)
            {
                case LuisResponseValue.LastPlayedChampion:
                    string lastPlayedChampion = _riotService.GetLastPlayedChampion();
                    yield return message.ReplyToChannel(lastPlayedChampion);
                    break;
                case LuisResponseValue.LastGameStats:
                    string statsForLastGame = _riotService.GetStatsOfLastGame();
                    yield return message.ReplyToChannel(statsForLastGame);
                    break;
                case LuisResponseValue.LastGameResult:
                    string resultOfLastGame = _riotService.GetResultOfLastGame();
                    yield return message.ReplyToChannel(resultOfLastGame);
                    break;
                default:
                    yield return message.ReplyToChannel("Sorry I'm not sure what you mean");
                    break;
            }
        }
    }
}