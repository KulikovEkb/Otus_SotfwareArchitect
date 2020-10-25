using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareArchitect.Auth.Api.Models.Requests;
using SoftwareArchitect.Auth.Api.Services;
using SoftwareArchitect.Auth.Api.Storage;

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
        public async Task<IActionResult> RegisterAsync([FromBody] CreateUserRequest request)
        {
            var createResult = await authService.RegisterAsync(request.ToUser()).ConfigureAwait(false);

            if (createResult.IsFailed)
                return StatusCode(500, createResult.Errors.First().Message);

            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] CreateUserRequest request)
        {
            var updateResult = await authService.SignInAsync(request.Username, request.Password).ConfigureAwait(false);

            if (updateResult.IsFailed)
                return StatusCode(500, updateResult.Errors.First().Message);


            return Ok();
        }

        [HttpPost("auth")]
        public async Task<ActionResult> AuthAsync([FromRoute] long userId)
        {
            Request.Cookies.TryGetValue("sessionId", out var sessionId);

            var userResult = await authService.AuthAsync(sessionId).ConfigureAwait(false);

            if (userResult.Value == null)
                return NotFound();

            return Ok();
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            Request.Cookies.TryGetValue("sessionId", out var sessionId);

            await authService.SignOutAsync(sessionId).ConfigureAwait(false);

            return NoContent();
        }
    }
}