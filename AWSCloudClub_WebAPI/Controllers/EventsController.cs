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

            var (eventId, errorMessage) = _eventBusiness.CreateEvent(eventItem);
            
            if (!string.IsNullOrEmpty(errorMessage))
                return BadRequest(errorMessage);
            
            return Ok(new { message = "Event added successfully.", eventId });
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
            var (events, errorMessage) = _eventBusiness.SearchEventByID(EventID);
            
            if (!string.IsNullOrEmpty(errorMessage))
                return BadRequest(errorMessage);
            
            if (events.Count == 0)
                return NotFound("Event not found.");
            
            return Ok(events[0]);
        }
    [HttpDelete("{eventId}")]
    public IActionResult RemoveEvent(string eventId)
        {
            if (string.IsNullOrEmpty(eventId))
            {
                return BadRequest("EventID is required.");
            }

            var (success, errorMessage) = _eventBusiness.RemoveEvent(eventId);
            
            if (success)
                return Ok("Event removed successfully.");
            
            return BadRequest(errorMessage);
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

            var (success, errorMessage) = await _eventBusiness.UpdateEventAsync(eventId, eventItem);
            
            if (success)
                return Ok("Event updated successfully.");
            
            return BadRequest(errorMessage);
        }
    }
}
