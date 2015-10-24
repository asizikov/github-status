using System;
using GitHub.Status.Model;
using Microsoft.Framework.Configuration;
using Nancy;
using Nancy.ModelBinding;

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
                var payload = this.Bind<Payload>();
                
                return payload.action;
                if(payload.issue.pull_request != null){
                    return payload.issue.body;
                }
                return key;
            };
        }
    }
}