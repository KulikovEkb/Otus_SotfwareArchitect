using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddDbContext<UserContext>(
                options => options
                    .UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ??
                               throw new Exception("connection string is wrong")),
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);
            services.AddSingleton<IUserStorage, UserStorage>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}