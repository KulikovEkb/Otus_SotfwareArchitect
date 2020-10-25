using System;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftwareArchitect.Auth.Api.Models;
using SoftwareArchitect.Auth.Api.Storage.Models;

namespace SoftwareArchitect.Auth.Api.Storage
{
    public class UserStorage : IUserStorage
    {
        private readonly UserContext userContext;
        private readonly ILogger logger;

        public UserStorage(UserContext userContext, ILogger logger)
        {
            this.userContext = userContext;
            this.logger = logger;
        }

        public Task<User> GetAsync(long userId)
        {
            logger.LogInformation($"Getting user {userId}");
            return userContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<User> GetAsync(string login, string password)
        {
            logger.LogInformation($"Getting user by login {login}");
            return userContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == login);
        }

        public async Task<Result<User>> CreateAsync(User user)
        {
            logger.LogInformation($"Creating user {user.Id}");
            try
            {
                var existingUser =
                    await userContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id).ConfigureAwait(false);

                if (existingUser != null)
                {
                    logger.LogInformation("User already exists");
                    return Result.Fail("User already exists");
                }

                logger.LogInformation("Existing user not found, so creating new one");
                await userContext.Users.AddAsync(user).ConfigureAwait(false);

                await userContext.SaveChangesAsync().ConfigureAwait(false);
                logger.LogInformation("User successfully created or updated");

                return Result.Ok(user);
            }
            catch (Exception exc)
            {
                logger.LogError($"Failed to create user {user.Id}: {exc.Message}");
                return Result.Fail(exc.Message);
            }
        }
    }
}