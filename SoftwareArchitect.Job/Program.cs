using System;
using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Common.Models;
using SoftwareArchitect.Storages.UserStorage;
using SoftwareArchitect.Storages.UserStorage.Models;

namespace SoftwareArchitect.Job
{
    public class Program
    {
        public static int Main()
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>()
                .UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ??
                           throw new Exception("connection string is wrong"));
            var userContext = new UserContext(optionsBuilder.Options);
            var userStorage = new UserStorage(userContext);

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

            return migrateResult.IsSuccess ? 0 : 1;
        }
    }
}