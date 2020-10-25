using System.Threading.Tasks;
using FluentResults;
using SoftwareArchitect.Auth.Api.Models;

namespace SoftwareArchitect.Auth.Api.Services
{
    internal interface IAuthService
    {
        Task<Result> RegisterAsync(UserCreds userCreds);

        Task<Result<string>> SignInAsync(string login, string password);

        void SignOut(string sessionId);

        Result<UserCreds> Auth(string sessionId);
    }
}