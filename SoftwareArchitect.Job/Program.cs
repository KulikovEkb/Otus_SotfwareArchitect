using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftwareArchitect.Common.Models;
using SoftwareArchitect.Storages.UserStorage;
using SoftwareArchitect.Storages.UserStorage.Models;

namespace SoftwareArchitect.Job
{
    public class Program
    {
        public static int Main()
        {
            var logger = InitLogger();
            logger.LogInformation("Migrating user");

            var optionsBuilder = new DbContextOptionsBuilder<UserContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ??
                           throw new Exception("connection string is wrong"));
            var userContext = new UserContext(optionsBuilder.Options);
            var userStorage = new UserStorage(userContext, logger);

            var migrationUser = new User
            {
                Id = 5,
                Username = "MigrationUser",
                FirstName = "MigrationUserName",
                LastName = "MigrationUserLastName",
                Email = "migration@migration.com"
            };

            var migrateResult =
                userStorage.CreateOrUpdateAsync(migrationUser).ConfigureAwait(false).GetAwaiter().GetResult();

            if (migrateResult.IsSuccess)
            {
                logger.LogInformation($"User {migrationUser.Id} successfully migrated");
                return 0;
            }

            logger.LogError($"Failed to migrate user {migrationUser.Id}: {migrateResult}");
            return 1;
        }

        private static ILogger<UserStorage> InitLogger()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            return loggerFactory.CreateLogger<UserStorage>();
        }
    }
}