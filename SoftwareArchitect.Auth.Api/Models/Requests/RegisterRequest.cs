using System.ComponentModel.DataAnnotations;

namespace SoftwareArchitect.Auth.Api.Models.Requests
{
    public class RegisterRequest
    {
        [Required, StringLength(256)] public string Login { get; set; }
        [Required, StringLength(256)] public string Password { get; set; }
        [StringLength(256)] public string FirstName { get; set; }
        [StringLength(256)] public string LastName { get; set; }
        [EmailAddress] public string Email { get; set; }
    }

    internal static class RegisterRequestExtensions
    {
        public static UserCreds ToUserCreds(this RegisterRequest request)
        {
            return new UserCreds
            {
                Login = request.Login,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };
        }
    }
}