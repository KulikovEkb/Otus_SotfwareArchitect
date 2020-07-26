using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SoftwareArchitect.Storages.UserStorage;

namespace SoftwareArchitect.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IUserStorage, DummyUserStorage>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}