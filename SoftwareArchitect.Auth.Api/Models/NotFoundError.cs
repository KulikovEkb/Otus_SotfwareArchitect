using FluentResults;

namespace SoftwareArchitect.Auth.Api.Models
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message) : base(message)
        {
        }
    }
}