namespace GitHub.Status.Service.Model
{
    public class Payload
    {
        public string action { get; set; }
        public Issue issue { get; set; }
        public Comment comment { get; set; }
        public Repository repository { get; set; }
        public Sender sender { get; set; }
    }
}