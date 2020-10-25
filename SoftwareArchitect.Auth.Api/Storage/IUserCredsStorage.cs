using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Auth.Api.Models;

namespace SoftwareArchitect.Auth.Api.Storage
{
    public interface IUserCredsStorage
    {
        Task<UserCreds> GetAsync(string login, string password);

        Task<Result<UserCreds>> CreateAsync(UserCreds userCreds);
    }
}