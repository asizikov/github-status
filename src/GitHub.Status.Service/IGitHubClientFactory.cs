using Octokit;

namespace GitHub.Status.Service
{
    public interface IGitHubClientFactory
    {
        IGitHubClient Create();
    }
}