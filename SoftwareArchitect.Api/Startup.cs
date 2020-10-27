using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using SoftwareArchitect.Api.Auth;
using SoftwareArchitect.Api.Auth.Authentication;
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
                               throw new Exception("connection string is wrong")));
            services.AddScoped<IUserStorage, UserStorage>();
            
            services
                .AddAuthentication(AuthConsts.Schemas.UserId)
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
                    AuthConsts.Schemas.UserId, null);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthConsts.Policies.UserId,
                    policy => policy.RequireClaim(AuthConsts.Claims.Types.UserId));
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseRouting();
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}