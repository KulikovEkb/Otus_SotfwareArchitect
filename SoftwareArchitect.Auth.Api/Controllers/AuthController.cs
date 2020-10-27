using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoftwareArchitect.Auth.Api.Models;
using SoftwareArchitect.Auth.Api.Models.Requests;
using SoftwareArchitect.Auth.Api.Services;

namespace SoftwareArchitect.Auth.Api.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ILogger logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            logger.LogInformation($"Registering user. Request: {JsonConvert.SerializeObject(request)}");
            var createResult = await authService.RegisterAsync(request.ToUserCreds()).ConfigureAwait(false);

            logger.LogInformation($"Registering result: {JsonConvert.SerializeObject(createResult)}");
            if (createResult.IsFailed)
                return StatusCode(500, createResult.Errors);

            return Ok(createResult.Value.Id);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            logger.LogInformation($"Signing in user. Request: {JsonConvert.SerializeObject(request)}");
            var signInResult = await authService.SignInAsync(request.Login, request.Password).ConfigureAwait(false);

            logger.LogInformation($"Sign in result: {JsonConvert.SerializeObject(signInResult)}");

            if (signInResult.HasError<NotFoundError>())
                return Unauthorized();

            if (signInResult.IsFailed)
                return StatusCode(500, signInResult.Errors);

            Response.Cookies.Append("session_id", signInResult.Value);

            return Ok();
        }

        [HttpPost("auth")]
        public IActionResult Auth()
        {
            Request.Cookies.TryGetValue("session_id", out var sessionId);

            if (string.IsNullOrWhiteSpace(sessionId))
                return Unauthorized();

            var authResult = authService.Auth(sessionId);

            logger.LogInformation($"Auth result: {JsonConvert.SerializeObject(authResult)}");

            if (authResult.HasError<NotFoundError>())
                return Unauthorized();

            Response.Headers.Add("X-UserId", authResult.Value.Id.ToString());
            logger.LogInformation($"Auth response: {JsonConvert.SerializeObject(Response.Headers)}");

            return Ok();
        }

        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            Request.Cookies.TryGetValue("session_id", out var sessionId);

            authService.SignOut(sessionId);

            return Ok();
        }
    }
}