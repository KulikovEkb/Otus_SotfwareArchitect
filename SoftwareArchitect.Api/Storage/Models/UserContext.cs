using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Api.Models;

namespace SoftwareArchitect.Api.Storage.Models
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