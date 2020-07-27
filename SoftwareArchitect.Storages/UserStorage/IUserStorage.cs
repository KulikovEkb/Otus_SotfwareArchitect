using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Common.Models;

namespace SoftwareArchitect.Storages.UserStorage
{
    public interface IUserStorage
    {
        Task<User> GetAsync(long userId);

        Task<Result<User>> CreateOrUpdateAsync(User user);

        Task DeleteAsync(long userId);
    }
}