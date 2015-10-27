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

        public string GitHubPassword
        {
            get { return Configuration["AppSettings:GitHubPassword"]; }
        }

        public string GitHubUserName
        {
            get { return Configuration["AppSettings:GitHubUserName"]; }
        }

        public string ReviewedMessage
        {
            get { return Configuration["AppSettings:ReviewedMessage"]; }
        }

        public int Threshold
        {
            get { return int.Parse(Configuration["AppSettings:Threshold"]); }
        }
    }
}