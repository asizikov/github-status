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
                var key = parameters.key as string;
                if(string.IsNullOrEmpty(key) || key != "secretKey"){
                    return HttpStatusCode.Forbidden;
                }
                return key;
            };
        }
    }
}
