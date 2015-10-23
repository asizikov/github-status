using System;
using Microsoft.Framework.Configuration;
using Nancy;

namespace GitHub.Status
{
    public class StatusModule : NancyModule
    {
        private IConfiguration Configuration { get; }
        private string Secret { get; }

        public StatusModule(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            Configuration = configuration;
            Secret = Configuration["AppSettings:GitHubIncomingKey"];

            Get["/"] = _ => "Hello Mac";

            Post["/github/status/{key}"] = parameters =>
            {
                var key = parameters.key.ToString();

                if (string.IsNullOrEmpty(key) || key != Secret)
                {
                    return HttpStatusCode.Forbidden;
                }
                return key;
            };
        }
    }
}