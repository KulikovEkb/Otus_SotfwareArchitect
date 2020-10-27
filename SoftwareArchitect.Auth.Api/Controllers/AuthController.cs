using System.Linq;
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
            logger.LogInformation($"Registering user. Request: {JsonConvert.SerializeObject(Request.Cookies)}");
            logger.LogInformation($"Registering user. Request: {JsonConvert.SerializeObject(request)}");
            var createResult = await authService.RegisterAsync(request.ToUserCreds()).ConfigureAwait(false);

            logger.LogInformation($"Registering result: {JsonConvert.SerializeObject(createResult)}");
            if (createResult.IsFailed)
                return StatusCode(500, createResult.Errors.First().Message);

            logger.LogInformation($"Registering response: {JsonConvert.SerializeObject(Response.Cookies)}");
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            logger.LogInformation($"Signing in user. Request: {JsonConvert.SerializeObject(Request.Cookies)}");
            logger.LogInformation($"Signing in user. Request: {JsonConvert.SerializeObject(request)}");
            var signInResult = await authService.SignInAsync(request.Login, request.Password).ConfigureAwait(false);

            logger.LogInformation($"Sign in result: {JsonConvert.SerializeObject(signInResult)}");

            if (signInResult.HasError<NotFoundError>())
                return NotFound();

            if (signInResult.IsFailed)
                return StatusCode(500, signInResult.Errors.First().Message);

            Response.Cookies.Append("sessionId", signInResult.Value);

            logger.LogInformation($"Sign in response: {JsonConvert.SerializeObject(Response.Cookies)}");

            return Ok();
        }

        [HttpPost("auth")]
        public IActionResult Auth()
        {
            logger.LogInformation($"Auth request: {JsonConvert.SerializeObject(Request.Cookies)}");
            Request.Cookies.TryGetValue("sessionId", out var sessionId);

            if (string.IsNullOrWhiteSpace(sessionId))
                return NotFound();

            var authResult = authService.Auth(sessionId);

            logger.LogInformation($"Auth result: {JsonConvert.SerializeObject(authResult)}");

            if (authResult.HasError<NotFoundError>())
                return NotFound();

            logger.LogInformation($"Auth response: {JsonConvert.SerializeObject(Response.Cookies)}");
            return Ok();
        }

        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            Request.Cookies.TryGetValue("sessionId", out var sessionId);

            authService.SignOut(sessionId);

            return Ok();
        }
    }
}