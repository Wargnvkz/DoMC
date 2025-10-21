using Microsoft.AspNetCore.Mvc;

namespace DoMCLib.Classes.Module.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private readonly Func<DoMCApplicationContext> _context;

        public ServerController(Func<DoMCApplicationContext> context)
        {
            _context = context;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new { status = "Server is running", timestamp = DateTime.UtcNow });
        }
    }
}
