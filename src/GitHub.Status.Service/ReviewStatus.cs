using Octokit;

namespace GitHub.Status.Service
{
    internal class ReviewStatus
    {
        public string Comment { get; set; }
        public CommitState Status { get; set; }
    }
}