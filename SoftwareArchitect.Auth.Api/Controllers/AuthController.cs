using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareArchitect.Auth.Api.Models;
using SoftwareArchitect.Auth.Api.Models.Requests;
using SoftwareArchitect.Auth.Api.Services;

namespace SoftwareArchitect.Auth.Api.Controllers
{
    internal class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var createResult = await authService.RegisterAsync(request.ToUserCreds()).ConfigureAwait(false);

            if (createResult.IsFailed)
                return StatusCode(500, createResult.Errors.First().Message);

            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            var signInResult = await authService.SignInAsync(request.Login, request.Password).ConfigureAwait(false);

            if (signInResult.HasError<NotFoundError>())
                return NotFound();

            if (signInResult.IsFailed)
                return StatusCode(500, signInResult.Errors.First().Message);

            Response.Cookies.Append("sessionId", signInResult.Value);
            return Ok();
        }

        [HttpPost("auth")]
        public ActionResult Auth()
        {
            Request.Cookies.TryGetValue("sessionId", out var sessionId);

            var userResult = authService.Auth(sessionId);

            if (userResult.Value == null)
                return NotFound();

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