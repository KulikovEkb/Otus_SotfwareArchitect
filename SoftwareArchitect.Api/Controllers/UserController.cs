using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoftwareArchitect.Api.Auth;
using SoftwareArchitect.Api.Models.Requests;
using SoftwareArchitect.Api.Models.Responses;
using SoftwareArchitect.Storages.UserStorage;
using IUserStorage = SoftwareArchitect.Api.Storage.IUserStorage;

namespace SoftwareArchitect.Api.Controllers
{
    [Route("user")]
    //[Authorize(Policy = AuthConsts.Policies.UserId)]
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
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(request)}");

            if (!User.HasClaim(AuthConsts.Claims.Types.UserId, request.Id.ToString()))
            {
                logger.LogInformation($"You have no access to {request.Id}");
                return StatusCode(403, $"You have no access to {request.Id}");
            }

            var createResult = await userStorage.CreateOrUpdateAsync(request.ToUser()).ConfigureAwait(false);

            var user = createResult.Value;

            return createResult.IsFailed
                ? StatusCode(500, createResult.Errors.First().Message)
                : Created(nameof(GetAsync), user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long userId, [FromBody] UpdateUserRequest request)
        {
            logger.LogInformation($"Create user request: {JsonConvert.SerializeObject(request)}");

            if (!User.HasClaim(AuthConsts.Claims.Types.UserId, userId.ToString()))
            {
                logger.LogInformation($"You have no access to {userId}");
                return StatusCode(403, $"You have no access to {userId}");
            }

            var updateResult = await userStorage.CreateOrUpdateAsync(request.ToUser(userId)).ConfigureAwait(false);

            if (updateResult.IsFailed)
                return StatusCode(500, updateResult.Errors.First().Message);

            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponse>> GetAsync([FromRoute] long userId)
        {
            if (!User.HasClaim(AuthConsts.Claims.Types.UserId, userId.ToString()))
            {
                logger.LogInformation($"You have no access to {userId}");
                return StatusCode(403, $"You have no access to {userId}");
            }

            var user = await userStorage.GetAsync(userId).ConfigureAwait(false);

            if (user == null)
                return NotFound();

            return Ok(user.ToGetUserResponse());
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] long userId)
        {
            if (!User.HasClaim(AuthConsts.Claims.Types.UserId, userId.ToString()))
            {
                logger.LogInformation($"You have no access to {userId}");
                return StatusCode(403, $"You have no access to {userId}");
            }

            await userStorage.DeleteAsync(userId).ConfigureAwait(false);

            return NoContent();
        }
    }
}