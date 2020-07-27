using System;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Common.Models;
using SoftwareArchitect.Storages.UserStorage.Models;

namespace SoftwareArchitect.Storages.UserStorage
{
    public class UserStorage : IUserStorage
    {
        private readonly UserContext userContext;

        public UserStorage(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public Task<User> GetAsync(long userId)
        {
            return userContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<Result<User>> CreateOrUpdateAsync(User user)
        {
            try
            {
                var existingUser =
                    await userContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id).ConfigureAwait(false);

                if (existingUser == null)
                    await userContext.Users.AddAsync(user).ConfigureAwait(false);
                else
                {
                    existingUser.Update(user);
                    userContext.Users.Update(existingUser);
                }

                await userContext.SaveChangesAsync().ConfigureAwait(false);

                return Result.Ok(user);
            }
            catch (Exception exc)
            {
                return Result.Fail(exc.Message);
            }
        }

        public async Task DeleteAsync(long userId)
        {
            var user = await userContext.Users.FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);

            if (user == null)
                return;

            userContext.Users.Remove(user);
            await userContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}