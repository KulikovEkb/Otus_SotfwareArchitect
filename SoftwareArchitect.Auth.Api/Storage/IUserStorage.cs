using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Auth.Api.Models;

namespace SoftwareArchitect.Auth.Api.Storage
{
    public interface IUserStorage
    {
        Task<User> GetAsync(long userId);

        Task<User> GetAsync(string login, string password);

        Task<Result<User>> CreateAsync(User user);
    }
}