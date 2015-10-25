using System;
using GitHub.Status.Model;
using GitHub.Status.Service;
using Microsoft.Framework.Configuration;
using Nancy;
using Nancy.ModelBinding;

namespace GitHub.Status
{
    public class StatusModule : NancyModule
    {
        private IConfiguration Configuration { get; }
        private string Secret { get; }
        private StatusSerivce GitHubStatus { get; }

        public StatusModule(IConfiguration configuration, StatusSerivce service)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            Configuration = configuration;
            Secret = Configuration["AppSettings:GitHubIncomingKey"];
            GitHubStatus = service;


            Get["/"] = _ => "Hello Mac";

            Post["/github/status/{key}", true] = async (parameters, token) =>
            {
                var key = parameters.key.ToString();

                if (string.IsNullOrEmpty(key) || key != Secret)
                {
                    return HttpStatusCode.Forbidden;
                }
                var payload = this.Bind<Payload>();
                if (payload.issue.html_url.Contains("/pull/"))
                {
                    await GitHubStatus.UpdateStatusAsync(payload);
                    return HttpStatusCode.OK;
                }
                if (payload.issue.pull_request != null)
                {
                    return payload.issue.body;
                }
                return key;
            };
        }
    }
}