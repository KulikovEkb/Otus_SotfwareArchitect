using System.ComponentModel.DataAnnotations;

namespace SoftwareArchitect.Auth.Api.Models.Requests
{
    public class SignInRequest
    {
        [Required, StringLength(256)] public string Login { get; set; }
        [Required, StringLength(256)] public string Password { get; set; }
    }
}