using Microsoft.AspNetCore.Mvc;
using AWSCloudClubEventManagement.Services;
using AWSCloudClub_Common.Models;

namespace AWSCloudClub_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly AWSCloudClub_BusinessLogic.ITicketBusinessLogic _ticketBusiness;

        public TicketsController(AWSCloudClub_BusinessLogic.ITicketBusinessLogic ticketBusiness)
        {
            _ticketBusiness = ticketBusiness;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTicket([FromBody] Tickets ticket)
        {
            if (ticket == null || string.IsNullOrWhiteSpace(ticket.TicketID))
                return BadRequest("Invalid ticket payload");

            var (success, error) = await _ticketBusiness.AddTicketAsync(ticket);
            if (!success)
            {
                if (error == "DuplicateTicketID" || error == "DuplicateTicketCode")
                    return Conflict(error);
                return BadRequest(error);
            }

            return CreatedAtAction(nameof(GetTicket), new { ticketId = ticket.TicketID }, ticket);
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetTicket(string ticketId)
        {
            var ticket = await _ticketBusiness.GetTicketByIdAsync(ticketId);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketBusiness.GetAllTicketsAsync();
            return Ok(tickets);
        }

        [HttpGet("byEvent/{eventId}")]
        public async Task<IActionResult> GetByEvent(string eventId)
        {
            var tickets = await _ticketBusiness.GetTicketsByEventAsync(eventId);
            return Ok(tickets);
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> Remove(string ticketId)
        {
            var ok = await _ticketBusiness.RemoveTicketAsync(ticketId);
            if (!ok) return BadRequest("Invalid ticket id");
            return Ok();
        }
    }
}
