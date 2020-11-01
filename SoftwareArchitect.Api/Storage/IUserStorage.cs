using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Api.Models;

namespace SoftwareArchitect.Api.Storage
{
    public interface IUserStorage
    {
        Task<User> GetAsync(long userId);

        Task<Result<User>> CreateOrUpdateAsync(User user);

        Task DeleteAsync(long userId);
    }
}