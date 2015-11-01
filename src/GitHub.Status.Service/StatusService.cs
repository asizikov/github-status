using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHub.Status.Service.Model;
using Octokit;

namespace GitHub.Status.Service
{
    public sealed class StatusService
    {
        private IGitHubClient Client { get; }
        private IStatusServiceConfiguration Configuration { get; }

        public StatusService(IStatusServiceConfiguration statusServiceConfiguration, IGitHubClientFactory clientFactory)
        {
            if (statusServiceConfiguration == null)
            {
                throw new ArgumentNullException(nameof(statusServiceConfiguration));
            }
            if (clientFactory == null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }
            Configuration = statusServiceConfiguration;

            Client = clientFactory.Create();
        }

        public async Task UpdateStatusAsync(Payload payload)
        {
            var status = await EvaluateReviewStatusAsync(payload, Configuration.Threshold).ConfigureAwait(false);
            await SendStatusAsync(payload, status).ConfigureAwait(false);
        }

        private async Task<ReviewStatus> EvaluateReviewStatusAsync(Payload payload, int threshold)
        {
            var owner = payload.repository.owner.login;
            var repositoryName = payload.repository.name;
            var issueNumber = payload.issue.number;
            var pullRequest =
                await Client.Repository.PullRequest.Get(owner, repositoryName, issueNumber).ConfigureAwait(false);
            if (pullRequest != null)
            {
                var comments =
                    await Client.Issue.Comment.GetAllForIssue(owner, repositoryName, issueNumber).ConfigureAwait(false);
                var reviewers = new HashSet<string>();
                foreach (var comment in comments.Where(comment => comment.Body.Contains(Configuration.ReviewedMessage)))
                {
                    reviewers.Add(comment.User.Login);
                }
                if (reviewers.Count > (threshold - 1))
                {
                    return new ReviewStatus { Comment = "Reviewed", Status = CommitState.Success };
                }
                return new ReviewStatus
                {
                    Comment = $"{reviewers.Count} signatures of {threshold} are done.",
                    Status = CommitState.Pending
                };
            }
            return new ReviewStatus
            {
                Comment = $"This request has to be signed by at least {threshold} reviewers",
                Status = CommitState.Pending
            };
        }

        private async Task SendStatusAsync(Payload payload, ReviewStatus status)
        {
            var owner = payload.repository.owner.login;
            var repo = payload.repository.name;
            var issueNumber = payload.issue.number;
            var commits = await Client.Repository.PullRequest.Commits(owner, repo, issueNumber).ConfigureAwait(false);
            var lastCommit = commits.Last();

            var commitStatus = new NewCommitStatus
            {
                Context = "code-review/treshold",
                State = status.Status,
                Description = status.Comment
            };
            await Client.Repository.CommitStatus.Create(owner, repo, lastCommit.Sha, commitStatus).ConfigureAwait(false);
            
            
            var newDeployment = new NewDeployment();
            newDeployment.Ref = lastCommit.Sha;
            newDeployment.Description = "Test enviromnemt";
            
            var deployment = await Client.Repository.Deployment.Create(owner,repo, newDeployment);
            var deploymentStatus = new NewDeploymentStatus();
            deploymentStatus.Description = "deployment started";
            deploymentStatus.State = DeploymentState.Failure;
            await Client.Repository.Deployment.Status.Create(owner,repo,deployment.Id,deploymentStatus);
        }
    }
}