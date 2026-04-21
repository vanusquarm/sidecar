using Microsoft.AspNetCore.Mvc;
using SidecarApp.Models;

namespace SidecarApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SidecarController : ControllerBase
    {
        private readonly ILogger<SidecarController> _logger;

        public SidecarController(ILogger<SidecarController> logger)
        {
            _logger = logger;
        }

        [HttpPost("log")]
        public IActionResult ReceiveLog([FromBody] LogEntry entry)
        {
            // In a real-world scenario, this sidecar would:
            // 1. Batch logs
            // 2. Forward to ElasticSearch, Datadog, or Azure Monitor
            // 3. Add enrichment data (container ID, node name, etc.)
            
            _logger.LogInformation(
                "[SIDECAR RECEIVE] From: {Source} | Severity: {Severity} | Msg: {Msg} | ReqId: {ReqId}",
                entry.SourceApplication,
                entry.Severity,
                entry.Message,
                entry.RequestId
            );

            return Accepted();
        }

        [HttpGet("health")]
        public IActionResult Health() => Ok(new { Status = "Healthy" });
    }
}
