using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Service.Users.Models;

namespace SoftwareArchitect.Service.Users.Storage
{
    public interface IUserStorage
    {
        Task<User> GetAsync(long userId);

        Task<Result<User>> CreateOrUpdateAsync(User user);

        Task DeleteAsync(long userId);
    }
}