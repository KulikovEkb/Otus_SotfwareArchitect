using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftwareArchitect.Auth.Api.Storage;
using SoftwareArchitect.Auth.Api.Storage.Models;

namespace SoftwareArchitect.Auth.Api
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
                               throw new Exception("connection string is wrong")));
            services.AddScoped<IUserStorage, UserStorage>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}