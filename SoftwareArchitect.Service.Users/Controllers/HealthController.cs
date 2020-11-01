using Microsoft.AspNetCore.Mvc;
using Prometheus;

namespace SoftwareArchitect.Service.Users.Controllers
{
    public class HealthController : ControllerBase
    {
        private static readonly Counter HealthRequestCounter = Metrics.CreateCounter(
            "myapp_health_requests_total", "Total number of health requests");

        [HttpGet("health")]
        public IActionResult CheckHealth()
        {
            HealthRequestCounter.Inc();
            return Ok(new {Status = "OK"});
        }
    }
}