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
}