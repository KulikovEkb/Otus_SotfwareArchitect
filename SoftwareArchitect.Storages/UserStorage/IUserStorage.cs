using System.Threading.Tasks;
using SoftwareArchitect.Common.Models;

namespace SoftwareArchitect.Storages.UserStorage
{
    public interface IUserStorage
    {
        Task<User> GetAsync(long userId);

        Task CreateOrUpdateAsync(User user);

        Task DeleteAsync(long userId);
    }

    // todo(kulikov): implement
    public class DummyUserStorage : IUserStorage
    {
        public Task<User> GetAsync(long userId)
        {
            return Task.FromResult<User>(null);
        }

        public Task CreateOrUpdateAsync(User user)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(long userId)
        {
            return Task.CompletedTask;
        }
    }
}