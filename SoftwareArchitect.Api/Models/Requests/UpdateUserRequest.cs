using SoftwareArchitect.Common.Models;

namespace SoftwareArchitect.Api.Models.Requests
{
    public class UpdateUserRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    internal static class UpdateUserRequestExtensions
    {
        public static User ToUser(this UpdateUserRequest request, long id)
        {
            return new User
            {
                Id = id,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };
        }
    }
}