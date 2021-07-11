using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Example.Elasticsearch.Api.Controllers
{
    [Route("[controller]")]
    public class ProbeController : ControllerBase
    {
        private readonly ILogger<ProbeController> _logger;

        public ProbeController(ILogger<ProbeController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            _logger.LogInformation("GET /probe");

            return Ok();
        }
    }
}
