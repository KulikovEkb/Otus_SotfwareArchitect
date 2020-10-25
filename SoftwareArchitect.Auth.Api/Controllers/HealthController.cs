using Microsoft.AspNetCore.Mvc;

namespace SoftwareArchitect.Auth.Api.Controllers
{
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult CheckHealth() => Ok(new {Status = "OK"});
    }
}