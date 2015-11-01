using GitHub.Status.Service;
using Microsoft.Framework.Configuration;

namespace GitHub.Status
{
    public class StatusServiceConfiguration : IStatusServiceConfiguration
    {
        private IConfiguration Configuration { get; }

        public StatusServiceConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GitHubPassword => Configuration["AppSettings:GitHubPassword"];

        public string GitHubUserName => Configuration["AppSettings:GitHubUserName"];

        public string ReviewedMessage => Configuration["AppSettings:ReviewedMessage"];

        public int Threshold => int.Parse(Configuration["AppSettings:Threshold"]);
    }
}