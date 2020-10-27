using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SoftwareArchitect.Api.Auth.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Headers.TryGetValue(AuthConsts.UserIdHeader, out var headerValues) ||
                !headerValues.Any())
            {
                return AuthenticateResult.Fail("Missing X-UserId");
            }

            if (!long.TryParse(headerValues.First(), out var userId) || userId == default)
                return AuthenticateResult.Fail("Value in X-UserId is not long");

            Logger.LogInformation($"Successfully authenticated {userId}");

            var claims = new List<Claim>
            {
                new Claim(AuthConsts.Claims.Types.UserId, userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);

            Context.User.AddIdentity(identity);

            var ticket = new AuthenticationTicket(Context.User, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}