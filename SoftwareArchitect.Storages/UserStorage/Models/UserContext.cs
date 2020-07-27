using Microsoft.EntityFrameworkCore;
using SoftwareArchitect.Common.Models;

namespace SoftwareArchitect.Storages.UserStorage.Models
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