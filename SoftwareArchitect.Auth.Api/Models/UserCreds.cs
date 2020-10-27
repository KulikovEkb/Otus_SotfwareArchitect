namespace SoftwareArchitect.Auth.Api.Models
{
    public class UserCreds
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}