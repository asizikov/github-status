using Octokit;
using GitHub.Status.Service;

namespace GitHub.Status
{
    public class GitHubClientFactory : IGitHubClientFactory
    {
        private IStatusServiceConfiguration Configuration {get;}
        public GitHubClientFactory(IStatusServiceConfiguration configuration)
        {
            Configuration = configuration;   
        }
        public IGitHubClient Create()
        {
            var client = new GitHubClient(new ProductHeaderValue("github-status"))
            {
                Credentials = new Credentials(Configuration.GitHubUserName, Configuration.GitHubPassword)
            };
            return client;
        }
    }
}