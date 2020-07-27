using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftwareArchitect.Storages.UserStorage;
using SoftwareArchitect.Storages.UserStorage.Models;

namespace SoftwareArchitect.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IUserStorage, UserStorage>();
            services.AddDbContext<UserContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}