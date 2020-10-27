using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoftwareArchitect.Api.Models.Requests;
using SoftwareArchitect.Api.Models.Responses;
using SoftwareArchitect.Storages.UserStorage;

namespace SoftwareArchitect.Api.Controllers
{
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IUserStorage userStorage;
        private readonly ILogger logger;

        public UserController(IUserStorage userStorage, ILogger<UserController> logger)
        {
            this.userStorage = userStorage;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
        {
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(Request.Cookies)}");
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(request)}");

            if (!Request.Cookies.TryGetValue("sessionId", out var sessionId))
            {
                logger.LogInformation("Session ID cookie not found");
                return Unauthorized("Session ID cookie not found");
            }

            logger.LogInformation($"Found session cookie {sessionId}");

            var createResult = await userStorage.CreateOrUpdateAsync(request.ToUser()).ConfigureAwait(false);

            var user = createResult.Value;

            return createResult.IsFailed
                ? StatusCode(500, createResult.Errors.First().Message)
                : Created(nameof(GetAsync), user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long userId, [FromBody] UpdateUserRequest request)
        {
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(Request.Cookies)}");
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(request)}");

            if (!Request.Cookies.TryGetValue("sessionId", out var sessionId))
            {
                logger.LogInformation("Session ID cookie not found");
                return Unauthorized("Session ID cookie not found");
            }

            logger.LogInformation($"Found session cookie {sessionId}");

            var updateResult = await userStorage.CreateOrUpdateAsync(request.ToUser(userId)).ConfigureAwait(false);

            if (updateResult.IsFailed)
                return StatusCode(500, updateResult.Errors.First().Message);

            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponse>> GetAsync([FromRoute] long userId)
        {
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(Request.Cookies)}");

            if (!Request.Cookies.TryGetValue("sessionId", out var sessionId))
            {
                logger.LogInformation("Session ID cookie not found");
                return Unauthorized("Session ID cookie not found");
            }

            logger.LogInformation($"Found session cookie {sessionId}");

            var user = await userStorage.GetAsync(userId).ConfigureAwait(false);

            if (user == null)
                return NotFound();

            return Ok(user.ToGetUserResponse());
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long userId)
        {
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(Request.Cookies)}");

            if (!Request.Cookies.TryGetValue("sessionId", out var sessionId))
            {
                logger.LogInformation("Session ID cookie not found");
                return Unauthorized("Session ID cookie not found");
            }

            logger.LogInformation($"Found session cookie {sessionId}");

            await userStorage.DeleteAsync(userId).ConfigureAwait(false);

            return NoContent();
        }
    }
}