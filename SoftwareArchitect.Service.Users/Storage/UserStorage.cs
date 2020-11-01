using System;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftwareArchitect.Service.Users.Models;
using SoftwareArchitect.Service.Users.Storage.Models;

namespace SoftwareArchitect.Service.Users.Storage
{
    public class UserStorage : IUserStorage
    {
        private readonly UserContext userContext;
        private readonly ILogger logger;

        public UserStorage(UserContext userContext, ILogger<UserStorage> logger)
        {
            this.userContext = userContext;
            this.logger = logger;
        }

        public Task<User> GetAsync(long userId)
        {
            logger.LogInformation($"Getting user {userId}");
            return userContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<Result<User>> CreateOrUpdateAsync(User user)
        {
            logger.LogInformation($"Creating or updating user {user.Id}");
            try
            {
                var existingUser =
                    await userContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id).ConfigureAwait(false);

                if (existingUser == null)
                {
                    logger.LogInformation("Existing user not found, so creating new one");
                    await userContext.Users.AddAsync(user).ConfigureAwait(false);
                }
                else
                {
                    logger.LogInformation("User found, updating");
                    existingUser.Update(user);
                    userContext.Users.Update(existingUser);
                }

                await userContext.SaveChangesAsync().ConfigureAwait(false);

                logger.LogInformation("User successfully created or updated");
                return Result.Ok(user);
            }
            catch (Exception exc)
            {
                logger.LogError($"Failed to create or update user {user.Id}: {exc.Message}");
                return Result.Fail(exc.Message);
            }
        }

        public async Task DeleteAsync(long userId)
        {
            logger.LogInformation($"Deleting user {userId}");
            var user = await userContext.Users.FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);

            if (user == null)
            {
                logger.LogInformation("User not found, nothing to delete");
                return;
            }

            userContext.Users.Remove(user);
            await userContext.SaveChangesAsync().ConfigureAwait(false);
            logger.LogInformation("User deleted");
        }
    }
}