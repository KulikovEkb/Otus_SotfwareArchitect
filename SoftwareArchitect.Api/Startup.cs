using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using SoftwareArchitect.Storages.UserStorage;
using SoftwareArchitect.Storages.UserStorage.Models;
using IUserStorage = SoftwareArchitect.Api.Storage.IUserStorage;
using UserContext = SoftwareArchitect.Api.Storage.Models.UserContext;
using UserStorage = SoftwareArchitect.Api.Storage.UserStorage;

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
                               throw new Exception("connection string is wrong")));
            services.AddScoped<IUserStorage, UserStorage>();
            
            /*services
                .AddAuthentication(AuthConsts.Schemas.UserId)
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    AuthConsts.Schemas.UserId, null);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthConsts.Policies.UserId,
                    policy => policy.RequireClaim(AuthConsts.Claims.Types.UserId));
            });*/
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            /*app.UseAuthentication();
            app.UseAuthorization();*/
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}