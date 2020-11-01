using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Service.Users.Models;

namespace SoftwareArchitect.Service.Users.Storage.Models
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