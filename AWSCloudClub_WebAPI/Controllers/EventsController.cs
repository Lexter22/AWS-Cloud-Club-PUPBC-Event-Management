using Microsoft.AspNetCore.Mvc;
using AWSCloudClubEventManagement.Services;
using AWSCloudClub_BusinessLogic;
using AWSCloudClub_Common.Models;

namespace AWSCloudClubEventManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventBusinessLogic _eventBusiness;

        public EventsController(EventBusinessLogic eventBusiness)
        {
            _eventBusiness = eventBusiness;
        }

        [HttpPost("add")]
        public IActionResult AddEvent(Events eventItem)
        {
            if (eventItem == null ||
                string.IsNullOrEmpty(eventItem.EventID) ||
                string.IsNullOrEmpty(eventItem.EventName) ||
                string.IsNullOrEmpty(eventItem.Description) ||
                eventItem.EventDate == default)
            {
                return BadRequest("Invalid event data.");
            }

            try
            {
                var id = _eventBusiness.CreateEvent(eventItem);
                if (string.IsNullOrEmpty(id)) return Conflict("EventID already exists or invalid.");
                return Ok(new { message = "Event added successfully.", eventId = id });
            }
            catch
            {
                return StatusCode(500, "Failed to add event.");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllEvents()
        {
            var events = _eventBusiness.GetAllEvents();
            return Ok(events);
        }

        [HttpGet("search")]
        public IActionResult SearchEventByID(string EventID)
        {
            if (string.IsNullOrEmpty(EventID))
            {
                return BadRequest("EventID parameter is required.");
            }
            var results = _eventBusiness.SearchEventByID(EventID);
            if (results != null && results.Count > 0)
            {
                return Ok(results[0]);
            }
            else
            {
                return NotFound("Event not found.");
            }
        }
    [HttpDelete("{eventId}")]
    public IActionResult RemoveEvent(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return BadRequest("EventID is required.");
            }

            try
            {
                _eventBusiness.RemoveEvent(eventId);
                return Ok("Event removed successfully.");
            }
            catch
            {
                return StatusCode(500, "Failed to remove event.");
            }
        }
    [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(string eventId, Events eventItem)
        {
            if (eventItem == null ||
                string.IsNullOrEmpty(eventId) ||
                string.IsNullOrEmpty(eventItem.EventName) ||
                string.IsNullOrEmpty(eventItem.Description) ||
                eventItem.EventDate == default)
            {
                return BadRequest("Invalid event data. EventID is required in the route and body should contain fields to update.");
            }

            try
            {
                var ok = await _eventBusiness.UpdateEventAsync(eventId, eventItem);
                if (ok) return Ok("Event updated successfully.");
                return NotFound("Event not found.");
            }
            catch
            {
                return StatusCode(500, "Failed to update event.");
            }
        }
    }
}
