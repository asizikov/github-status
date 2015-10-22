namespace GitHub.Status
{
    using Microsoft.AspNet.Builder;
    using Nancy.Owin;
    using Microsoft.AspNet.Hosting;
    using Microsoft.Dnx.Runtime;
    using Microsoft.Framework.Configuration;

    public class Startup
    {
        public Startup(IHostingEnvironment env,IApplicationEnvironment appEnv){
            var builder = new ConfigurationBuilder()
        .SetBasePath(appEnv.ApplicationBasePath)
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
        builder.AddEnvironmentVariables();
        var configuration = builder.Build();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
        }
    }
}
