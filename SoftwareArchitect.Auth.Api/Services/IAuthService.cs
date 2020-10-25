using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using SoftwareArchitect.Auth.Api.Models;
using SoftwareArchitect.Auth.Api.Storage;

namespace SoftwareArchitect.Auth.Api.Services
{
    internal interface IAuthService
    {
        Task<Result> RegisterAsync(User user);

        Task<Result<string>> SignInAsync(string login, string password);

        Task SignOutAsync(string sessionId);

        Task<Result<User>> AuthAsync(string sessionId);
    }

    internal class AuthService : IAuthService
    {
        private static readonly ConcurrentDictionary<string, User> Sessions;

        private readonly IUserStorage userStorage;
        private readonly ILogger logger;

        static AuthService() => Sessions = new ConcurrentDictionary<string, User>();

        public AuthService(IUserStorage userStorage, ILogger logger)
        {
            this.userStorage = userStorage;
            this.logger = logger;
        }

        public async Task<Result> RegisterAsync(User user)
        {
            return await userStorage.CreateAsync(user).ConfigureAwait(false);
        }

        public async Task<Result<string>> SignInAsync(string login, string password)
        {
            var getResult = await userStorage.GetAsync(login, password).ConfigureAwait(false);

            if (getResult == null)
                return Result.Fail("Not found");

            var sessionId = $"{getResult.FirstName}{getResult.Id}{getResult.LastName}";
            Sessions[sessionId] = getResult;

            return Result.Ok(sessionId);
        }

        public async Task SignOutAsync(string sessionId)
        {
        }

        public async Task<Result<User>> AuthAsync(string sessionId)
        {
            if (Sessions.TryGetValue(sessionId, out var user))
                return Result.Ok(user);

            return Result.Fail("Session not found");
        }
    }
}