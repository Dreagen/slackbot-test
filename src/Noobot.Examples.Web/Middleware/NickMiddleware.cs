using System.Collections.Generic;
using Noobot.Core.MessagingPipeline.Middleware;
using Noobot.Core.MessagingPipeline.Request;
using Noobot.Core.MessagingPipeline.Response;
using Noobot.Core.Plugins.StandardPlugins;
using Noobot.Examples.ConsoleService.LuisAi;
using Noobot.Examples.ConsoleService.RiotApi;

namespace Noobot.Toolbox.Pipeline.Middleware
{
    public class NickMiddleware : MiddlewareBase
    {
        private readonly StatsPlugin _statsPlugin;
        private LuisAiService _luisAiService;
        private RiotService _riotService;

        public NickMiddleware(IMiddleware next, StatsPlugin statsPlugin) : base(next)
        {
            _statsPlugin = statsPlugin;
            _luisAiService = new LuisAiService();
            _riotService = new RiotService();
            HandlerMappings = new[]
            {
                new HandlerMapping
                {
                    ValidHandles = new [] { "" },
                    Description = "Books holiday",
                    EvaluatorFunc = HolidayHandler,
                    MessageShouldTargetBot = true
                }
            };
        }

        private IEnumerable<ResponseMessage> HolidayHandler(IncomingMessage message, string matchedHandle)
        {
            yield return message.IndicateTypingOnChannel();

            LuisIntent result = _luisAiService.GetResponseFromMessage(message.FullText);

            if (result.LuisResponse == LuisResponseValue.LastPlayedChampion)
            {
                string lastPlayedChampion = _riotService.GetLastPlayedChampion();
                yield return message.ReplyToChannel(lastPlayedChampion);
                yield return message.ReplyToChannel("Score: " + result.Score);
            }
            else
            {
                yield return message.ReplyToChannel("Sorry I'm not sure what you mean");
                yield return message.ReplyToChannel("Score: " + result.Score);
            }
        }
    }
}