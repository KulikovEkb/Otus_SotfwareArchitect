using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SoftwareArchitect
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet(
                    "/health/",
                    async context =>
                    {
                        await context.Response.WriteAsync("{\"status\": \"OK\"}").ConfigureAwait(false);
                    });
            });
        }
    }
}