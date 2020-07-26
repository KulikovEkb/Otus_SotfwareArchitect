using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoftwareArchitect.Api.Models.Requests;
using SoftwareArchitect.Api.Models.Responses;
using SoftwareArchitect.Storages.UserStorage;

namespace SoftwareArchitect.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserStorage userStorage;

        public UserController(IUserStorage userStorage)
        {
            this.userStorage = userStorage;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request)
        {
            await userStorage.CreateOrUpdateAsync(request.ToUser()).ConfigureAwait(false);

            return CreatedAtAction(nameof(GetAsync), new {userId = request.Id}, request);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] long userId, [FromBody] UpdateUserRequest request)
        {
            var existingUser = await userStorage.GetAsync(userId).ConfigureAwait(false);
            if (existingUser == null)
                return NotFound();

            await userStorage.CreateOrUpdateAsync(request.ToUser(userId)).ConfigureAwait(false);

            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponse>> GetAsync(long userId)
        {
            var user = await userStorage.GetAsync(userId).ConfigureAwait(false);

            if (user == null)
                return NotFound();

            return Ok(user.ToGetUserResponse());
        }

        [HttpDelete("{userId")]
        public async Task<IActionResult> DeleteAsync(long userId)
        {
            await userStorage.DeleteAsync(userId).ConfigureAwait(false);

            return NoContent();
        }
    }
}