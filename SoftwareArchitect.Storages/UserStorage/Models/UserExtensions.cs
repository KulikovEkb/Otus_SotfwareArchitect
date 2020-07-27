using SoftwareArchitect.Common.Models;

namespace SoftwareArchitect.Storages.UserStorage.Models
{
    internal static class UserExtensions
    {
        public static void Update(this User user, User newUser)
        {
            if (!string.Equals(user.Username, newUser.Username))
                user.Username = newUser.Username;
            if (!string.Equals(user.FirstName, newUser.FirstName))
                user.FirstName = newUser.FirstName;
            if (!string.Equals(user.LastName, newUser.LastName))
                user.LastName = newUser.LastName;
            if (!string.Equals(user.Email, newUser.Email))
                user.Email = newUser.Email;
            if (!string.Equals(user.Phone, newUser.Phone))
                user.Phone = newUser.Phone;
        }
    }
}