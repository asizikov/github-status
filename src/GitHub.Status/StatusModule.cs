using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitHub.Status.Model;
using Microsoft.Framework.Configuration;
using Nancy;
using Nancy.ModelBinding;
using Octokit;

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
                    var namd = await ReadyToMerge(payload, 1);
                    var result = await SendStatus(payload);
                    return result;
                }
                if (payload.issue.pull_request != null)
                {
                    return payload.issue.body;
                }
                return key;
            };
        }

        private async Task<string> SendStatus(Payload payload)
        {
            var client = new GitHubClient(new ProductHeaderValue("github-status"))
            {
                Credentials =
                    new Credentials(
                        Configuration["AppSettings:GitHubUserName"],
                        Configuration["AppSettings:GitHubPassword"])
            };
            var owner = payload.repository.owner.login;
            var repo = payload.repository.name;
            var repository = await client.Repository.Get(owner, repo);
            var issueNumber = payload.issue.number;
            var commits = await client.Repository.PullRequest.Commits(owner, repo, issueNumber);
            var lastCommit = commits.Last();
            return lastCommit.Commit.Ref;
            var status = new NewCommitStatus
            {
                Context = "code-review/treshold",
                State = CommitState.Pending,
                Description = "Check if there were enough :shipit:vcomments made"
            };

            await client.Repository.CommitStatus.Create(owner, repo, lastCommit.Commit.Ref, status);
            return string.Empty;
        }

        private async Task<string> ReadyToMerge(Payload payload, int threshold)
        {
            var client = new GitHubClient(new ProductHeaderValue("github-status"))
            {
                Credentials =
                    new Credentials(
                        Configuration["AppSettings:GitHubUserName"],
                        Configuration["AppSettings:GitHubPassword"])
            };
            var owner = payload.repository.owner.login;
            var repo = payload.repository.name;
            var issueNumber = payload.issue.number;
            var repository = await client.Repository.Get(owner, repo);
            var pullRequest = await client.Repository.PullRequest.Get(owner, repo, issueNumber);
            if (pullRequest != null)
            {
                var comments = await client.Issue.Comment.GetAllForIssue(owner, repo, issueNumber);
                var users = new HashSet<string>();
                foreach (var comment in comments)
                {
                    if (comment.Body.Contains(":shipit:"))
                    {
                        users.Add(comment.User.Name + ":" + comment.User.Login);
                    }
                }
                return users.Count > (threshold - 1) ? "can be merged" : "not ready yet";
            }
            return pullRequest.Body;
        }
    }
}