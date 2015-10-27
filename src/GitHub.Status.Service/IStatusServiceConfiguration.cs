namespace GitHub.Status.Service
{
    public interface IStatusServiceConfiguration
    {
        string GitHubUserName { get; }
        string GitHubPassword { get; }
        string ReviewedMessage { get; }
        int Threshold { get; }
    }
}