using Noobot.Core.Configuration;
using Noobot.Toolbox.Pipeline.Middleware;

namespace Noobot.Examples.ConsoleService.Configuration
{
    public class ExampleConfiguration : ConfigurationBase
    {
        public ExampleConfiguration()
        {
            UseMiddleware<WelcomeMiddleware>();
            UseMiddleware<JokeMiddleware>();
            UseMiddleware<CalculatorMiddleware>();
            UseMiddleware<FlickrMiddleware>();
            UseMiddleware<NickMiddleware>();

            UsePlugin<Toolbox.Plugins.StoragePlugin>();
        }
    }
}