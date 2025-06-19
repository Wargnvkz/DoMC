using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCLib.Classes.Module.API.Controllers
{
    [Route("api/[controller]")] // Маршрут: /api/status
    [ApiController] // Упрощает обработку APIModule (валидация, JSON)
    public class StatusController : ControllerBase
    {
        [HttpGet] // GET /api/status
        public IActionResult Get()
        {
            return Ok(new { status = "Running", timestamp = DateTime.UtcNow });
        }

        [HttpGet("{id}")] // GET /api/status/123
        public IActionResult GetById(int id)
        {
            return Ok(new { status = $"Running with ID {id}", timestamp = DateTime.UtcNow });
        }

        [HttpPost] // POST /api/status
        public IActionResult Post([FromBody] StatusRequest request)
        {
            return Ok(new { message = $"Received status: {request.Status}" });
        }        
    }

    public class StatusRequest
    {
        public string Status { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { status = "Server is running", timestamp = DateTime.UtcNow });
        }
    }
}
