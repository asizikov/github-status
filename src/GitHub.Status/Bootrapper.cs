using Microsoft.Framework.Configuration;
using Nancy;
using Nancy.TinyIoc;

namespace GitHub.Status
{
    public class Bootrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            var builder = new ConfigurationBuilder().AddJsonFile("config.json");
            builder.AddEnvironmentVariables();
            var configuration = builder.Build();
            container.Register<IConfiguration>(configuration);
        }
    }
}