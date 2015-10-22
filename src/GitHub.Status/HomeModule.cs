using Nancy;

namespace GitHub.Status
{
    public class StatusModule : NancyModule
    {
        public StatusModule()
        {
            Get["/"] = _ => {
             return "Hello Mac";
            };
                        
            Post["/github/status/{key}"] = parameters => {
                var key = parameters.key;
                return key;
            };
        }
    }
}
