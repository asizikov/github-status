using Microsoft.AspNet.Builder;
using Nancy.Owin;

namespace GitHub.Status
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app) => app.UseOwin(x => x.UseNancy());
    }
}