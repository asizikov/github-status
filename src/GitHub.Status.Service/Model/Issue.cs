using System.Collections.Generic;

namespace GitHub.Status.Service.Model
{
    public class Issue
    {
        public string url { get; set; }
        public string labels_url { get; set; }
        public string comments_url { get; set; }
        public string events_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public List<object> labels { get; set; }
        public string state { get; set; }
        public bool locked { get; set; }
        public object assignee { get; set; }
        public object milestone { get; set; }
        public int comments { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public object closed_at { get; set; }
        public PullRequest pull_request { get; set; }
        public string body { get; set; }
    }
}