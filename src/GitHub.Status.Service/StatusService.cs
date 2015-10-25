using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHub.Status.Service.Model;
using Octokit;

namespace GitHub.Status.Service
{
    public class StatusSerivce
    {
        private IGitHubClient Client { get; }
        private IStatusServiceConfiguration Configuration { get; }
        public StatusSerivce(IStatusServiceConfiguration statusServiceConfiguration)
        {
            if (statusServiceConfiguration == null)
            {
                throw new ArgumentNullException(nameof(statusServiceConfiguration));
            }
            Configuration = statusServiceConfiguration;

            Client = new GitHubClient(new ProductHeaderValue("github-status"))
            {
                Credentials =
                    new Credentials(
                        Configuration.GitHubUserName,
                        Configuration.GitHubPassword)
            };
        }

        public async Task UpdateStatusAsync(Payload payload)
        {
            var status = await EvaluateReviewStatusAsync(payload, Configuration.Treshold).ConfigureAwait(false);
            await SendStatusAsync(payload, status).ConfigureAwait(false);
        }

        private async Task<ReviewStatus> EvaluateReviewStatusAsync(Payload payload, int threshold)
        {
            var owner = payload.repository.owner.login;
            var repo = payload.repository.name;
            var issueNumber = payload.issue.number;
            var pullRequest = await Client.Repository.PullRequest.Get(owner, repo, issueNumber).ConfigureAwait(false);
            if (pullRequest != null)
            {
                var comments = await Client.Issue.Comment.GetAllForIssue(owner, repo, issueNumber).ConfigureAwait(false);
                var reviewers = new HashSet<string>();
                foreach (var comment in comments)
                {
                    if (comment.Body.Contains(Configuration.ReviewedMessage))
                    {
                        reviewers.Add(comment.User.Login);
                    }
                }
                if (reviewers.Count > (threshold - 1))
                {
                    return new ReviewStatus
                    {
                        Comment = "Reviewed",
                        Status = CommitState.Success
                    };
                }
                return new ReviewStatus
                {
                    Comment = $"{reviewers.Count} signatures of {threshold} are done.",
                    Status = CommitState.Pending
                };
            }
            return new ReviewStatus
            {
                Comment = $"This request has to be signed by {threshold} reviewers",
                Status = CommitState.Pending
            };
        }

        private async Task SendStatusAsync(Payload payload, ReviewStatus status)
        {
            var owner = payload.repository.owner.login;
            var repo = payload.repository.name;
            var repository = await Client.Repository.Get(owner, repo);
            var issueNumber = payload.issue.number;
            var commits = await Client.Repository.PullRequest.Commits(owner, repo, issueNumber);
            var lastCommit = commits.Last();

            var commitStatus = new NewCommitStatus
            {
                Context = "code-review/treshold",
                State = status.Status,
                Description = status.Comment,

            };
            await Client.Repository.CommitStatus.Create(owner, repo, lastCommit.Sha, commitStatus);
        }
    }
}