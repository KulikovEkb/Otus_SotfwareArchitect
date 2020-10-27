using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Auth.Api.Models;

namespace SoftwareArchitect.Auth.Api.Storage.Models
{
    public class UserCredsContext : DbContext
    {
        public UserCredsContext(DbContextOptions<UserCredsContext> options) : base(options)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Database.EnsureCreated();
        }

        public DbSet<UserCreds> UsersCreds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}