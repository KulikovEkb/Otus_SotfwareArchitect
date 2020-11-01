using System.ComponentModel.DataAnnotations;

namespace SoftwareArchitect.Api.Models.Requests
{
    public class CreateUserRequest
    {
        public long Id { get; set; }
        [Required, StringLength(256)] public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress] public string Email { get; set; }
        [Phone] public string Phone { get; set; }
    }

    internal static class CreateUserRequestExtensions
    {
        public static User ToUser(this CreateUserRequest request)
        {
            return new User
            {
                Id = request.Id,
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };
        }
    }
}