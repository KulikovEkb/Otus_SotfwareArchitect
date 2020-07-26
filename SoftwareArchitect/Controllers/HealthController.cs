using Microsoft.AspNetCore.Mvc;

namespace SoftwareArchitect.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [Route("health")]
        public IActionResult CheckHealth() => Ok(new {Status = "OK"});
    }
}