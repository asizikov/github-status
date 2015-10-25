using System;
using Microsoft.Framework.Configuration;
using GitHub.Status.Service;

namespace GitHub.Status{
	public class StatusServiceConfiguration : IStatusServiceConfiguration{
		private IConfiguration Configuration {get;}
		public StatusServiceConfiguration(IConfiguration configuration){
			Configuration = configuration;
		}

        public string GitHubPassword
        {
            get
            {
                return Configuration["AppSettings:GitHubPassword"];
            }
        }

        public string GitHubUserName
        {
            get
            {
                return Configuration["AppSettings:GitHubUserName"];
            }
        }

        public string ReviewedMessage
        {
            get
            {
                return Configuration["AppSettings:ReviewedMessage"];
            }
        }

        public int Treshold
        {
            get
            {
                
                return int.Parse(Configuration["AppSettings:Treshold"]);
            }
        }
    }
}