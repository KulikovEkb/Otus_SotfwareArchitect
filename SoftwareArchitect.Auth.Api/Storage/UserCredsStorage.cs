using System;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftwareArchitect.Auth.Api.Models;
using SoftwareArchitect.Auth.Api.Storage.Models;

namespace SoftwareArchitect.Auth.Api.Storage
{
    public class UserCredsStorage : IUserCredsStorage
    {
        private readonly UserCredsContext userCredsContext;
        private readonly ILogger logger;

        public UserCredsStorage(UserCredsContext userCredsContext, ILogger<UserCredsStorage> logger)
        {
            this.userCredsContext = userCredsContext;
            this.logger = logger;
        }

        public Task<UserCreds> GetAsync(string login, string password)
        {
            logger.LogInformation($"Getting user credentials by login {login}");
            return userCredsContext.UsersCreds
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
        }

        public async Task<Result<UserCreds>> CreateAsync(UserCreds userCreds)
        {
            logger.LogInformation("Saving user credentials");
            try
            {
                var existingCreds = await userCredsContext.UsersCreds
                    .Where(x => x.Login == userCreds.Login)
                    .ToArrayAsync()
                    .ConfigureAwait(false);

                if (existingCreds.Length > 0)
                {
                    logger.LogInformation("User already exists");
                    return Result.Fail("User already exists");
                }

                logger.LogInformation("Existing user credentials not found, so creating new one");
                await userCredsContext.UsersCreds.AddAsync(userCreds).ConfigureAwait(false);

                await userCredsContext.SaveChangesAsync().ConfigureAwait(false);
                logger.LogInformation("User credentials successfully created or updated");

                return Result.Ok(userCreds);
            }
            catch (Exception exc)
            {
                logger.LogError($"Failed to create user credentials: {exc.Message}");
                return Result.Fail(exc.Message);
            }
        }
    }
}