using System.Threading.Tasks;
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

        public async Task CreateOrUpdateAsync(User user)
        {
            var existingUser = await userContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id).ConfigureAwait(false);

            if (existingUser == null)
                await userContext.Users.AddAsync(user).ConfigureAwait(false);
            else
                userContext.Users.Update(user);

            await userContext.SaveChangesAsync().ConfigureAwait(false);
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