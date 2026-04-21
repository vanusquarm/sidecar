using Microsoft.AspNetCore.Mvc;
using MainApp.Models;
using System.Net.Http.Json;

namespace MainApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusinessController : ControllerBase
    {
        private readonly HttpClient _sidecarClient;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IHttpClientFactory httpClientFactory, ILogger<BusinessController> logger)
        {
            _sidecarClient = httpClientFactory.CreateClient("SidecarClient");
            _logger = logger;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessData([FromBody] BusinessData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Message))
            {
                return BadRequest("Invalid business data.");
            }

            // 1. Perform "Core Business Logic"
            _logger.LogInformation("Processing business data: {Id}", data.Id);
            var result = new 
            {
                Status = "Success",
                ProcessedAt = DateTime.UtcNow,
                OriginalData = data
            };

            // 2. Offload cross-cutting concern (Logging/Telemetry) to the Sidecar
            try
            {
                var logEntry = new
                {
                    SourceApplication = "MainApp",
                    Severity = "Information",
                    Message = $"Successfully processed business entity {data.Id}",
                    RequestId = HttpContext.TraceIdentifier
                };

                // We don't want to block the main response if logging fails, 
                // but for this demo we await to show the interaction.
                var response = await _sidecarClient.PostAsJsonAsync("/api/sidecar/log", logEntry);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Sidecar returned non-success code: {Code}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // In a production app, you might queue this locally if the sidecar is down
                _logger.LogError(ex, "Failed to communicate with sidecar.");
            }

            return Ok(result);
        }
    }
}
