using System.ComponentModel.DataAnnotations;

namespace SoftwareArchitect.Api.Models.Requests
{
    public class UpdateUserRequest
    {
        [Required, StringLength(256)] public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress] public string Email { get; set; }
        [Phone] public string Phone { get; set; }
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