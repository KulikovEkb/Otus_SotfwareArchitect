using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Auth.Api.Models;

namespace SoftwareArchitect.Auth.Api.Services
{
    public interface IAuthService
    {
        Task<Result<UserCreds>> RegisterAsync(UserCreds userCreds);

        Task<Result<string>> SignInAsync(string login, string password);

        void SignOut(string sessionId);

        Result<UserCreds> Auth(string sessionId);
    }
}