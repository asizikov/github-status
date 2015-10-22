using Nancy;

namespace GitHub.Status
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
             return "Hello Mac";
            };
                        
            Get["/github/status/{key}"] = parameters => {
                return "parameters.key";
            };
        }
    }
}
