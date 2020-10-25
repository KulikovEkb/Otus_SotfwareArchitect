using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Auth.Api.Models;

namespace SoftwareArchitect.Auth.Api.Storage.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
    }
}