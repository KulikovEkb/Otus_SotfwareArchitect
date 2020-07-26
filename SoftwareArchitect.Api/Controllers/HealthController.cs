using Microsoft.AspNetCore.Mvc;

namespace SoftwareArchitect.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [Route("health")]
        public IActionResult CheckHealth() => Ok(new {Status = "OK"});
    }
}