using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Extensions.Logging;
using SoftwareArchitect.Auth.Api.Models;
using SoftwareArchitect.Auth.Api.Storage;

namespace SoftwareArchitect.Auth.Api.Services
{
    internal class AuthService : IAuthService
    {
        private static readonly ConcurrentDictionary<string, UserCreds> Sessions;

        private readonly IUserCredsStorage userCredsStorage;
        private readonly ILogger logger;

        static AuthService() => Sessions = new ConcurrentDictionary<string, UserCreds>();

        public AuthService(IUserCredsStorage userCredsStorage, ILogger logger)
        {
            this.userCredsStorage = userCredsStorage;
            this.logger = logger;
        }

        public async Task<Result> RegisterAsync(UserCreds userCreds) =>
            await userCredsStorage.CreateAsync(userCreds).ConfigureAwait(false);

        public async Task<Result<string>> SignInAsync(string login, string password)
        {
            var getCredsResult = await userCredsStorage.GetAsync(login, password).ConfigureAwait(false);

            if (getCredsResult == null)
                return Result.Fail(new NotFoundError($"No creds found for login {login}"));

            var sessionId = $"{getCredsResult.Login}{DateTime.UtcNow.Ticks}";
            Sessions[sessionId] = getCredsResult;

            return Result.Ok(sessionId);
        }

        public void SignOut(string sessionId) => Sessions.TryRemove(sessionId, out _);

        public Result<UserCreds> Auth(string sessionId)
        {
            if (Sessions.TryGetValue(sessionId, out var userCreds))
                return Result.Ok(userCreds);

            return Result.Fail("Session not found");
        }
    }
}