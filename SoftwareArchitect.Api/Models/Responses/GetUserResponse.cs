namespace SoftwareArchitect.Api.Models.Responses
{
    public class GetUserResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    internal static class GetUserResponseExtensions
    {
        public static GetUserResponse ToGetUserResponse(this User user)
        {
            return new GetUserResponse
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone
            };
        }
    }
}