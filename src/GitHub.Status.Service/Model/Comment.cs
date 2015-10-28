namespace GitHub.Status.Service.Model
{
    public class Comment
    {
        public string url { get; set; }
        public string html_url { get; set; }
        public string issue_url { get; set; }
        public int id { get; set; }
        public User2 user { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string body { get; set; }
    }
}