using Microsoft.AspNetCore.Mvc;
using AWSCloudClub_Common.Models;
using AWSCloudClub_BusinessLogic;

namespace AWSCloudClub_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventEmailTestController : ControllerBase
    {
        private readonly EventEmailService _emailService;

        public EventEmailTestController(EventEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async System.Threading.Tasks.Task<IActionResult> SendTestEmail([FromBody] Events ev)
        {
            if (ev == null) return BadRequest("Event payload required.");

            try
            {
                await _emailService.SendEmailAsync(ev);
                return Ok(new { message = "Email triggered" });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
