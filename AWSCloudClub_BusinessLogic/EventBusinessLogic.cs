using AWSCloudClub_Common.Models;
using AWSCloudClubEventManagement.Services;

namespace AWSCloudClub_BusinessLogic
{
    public class EventBusinessLogic
    {
        private readonly EventDataService _dataService;

        public EventBusinessLogic(EventDataService dataService)
        {
            _dataService = dataService;
        }

        public List<Events> GetAllEvents() => _dataService.GetAllEvents();

        public Task<List<Events>> GetAllEventsAsync() => _dataService.GetAllEventsAsync();

        public (string EventId, string ErrorMessage) CreateEvent(Events ev)
        {
            if (ev == null)
                return (string.Empty, "Event data is required.");
            
            if (string.IsNullOrWhiteSpace(ev.EventID))
                return (string.Empty, "EventID is required.");
            
            if (string.IsNullOrWhiteSpace(ev.EventName))
                return (string.Empty, "Event name is required.");
            
            if (string.IsNullOrWhiteSpace(ev.EventDate))
                return (string.Empty, "Event date is required.");
            
            if (string.IsNullOrWhiteSpace(ev.Description))
                return (string.Empty, "Event description is required.");

            // ensure EventID is unique
            var existing = _dataService.SearchEventByID(ev.EventID);
            if (existing != null && existing.Count > 0)
                return (string.Empty, "EventID already exists.");

            _dataService.AddEvent(ev);
            return (ev.EventID, string.Empty);
        }

        public async Task<string> AddEventAsync(Events ev)
        {
            if (ev == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(ev.EventID) || string.IsNullOrWhiteSpace(ev.EventName)) return string.Empty;

            var existing = _dataService.SearchEventByID(ev.EventID);
            if (existing != null && existing.Count > 0) return string.Empty;

            await _dataService.AddEventAsync(ev);
            return ev.EventID;
        }

        public (bool Success, string ErrorMessage) RemoveEvent(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                return (false, "EventID is required.");
            
            _dataService.RemoveEvent(eventId);
            return (true, string.Empty);
        }

        public (bool Success, string ErrorMessage) UpdateEvent(Events ev)
        {
            if (ev == null)
                return (false, "Event data is required.");
            
            if (string.IsNullOrWhiteSpace(ev.EventID))
                return (false, "EventID is required.");
            
            if (string.IsNullOrWhiteSpace(ev.EventName))
                return (false, "Event name is required.");

            _dataService.UpdateEvent(ev);
            return (true, string.Empty);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateEventAsync(string eventId, Events ev)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                return (false, "EventID is required.");
            
            if (ev == null)
                return (false, "Event data is required.");

            bool success = await _dataService.UpdateEventAsync(eventId, ev);
            return success ? (true, string.Empty) : (false, "Event not found or update failed.");
        }

        public (List<Events> Events, string ErrorMessage) SearchEventByID(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                return (new List<Events>(), "EventID is required.");
            
            var events = _dataService.SearchEventByID(eventId);
            return (events, string.Empty);
        }

        private string GenerateUniqueEventId()
        {
            var rng = new System.Random();
            for (int i = 0; i < 10; i++)
            {
                var id = RandomString(8, rng);
                var found = _dataService.SearchEventByID(id);
                if (found == null || found.Count == 0) return id;
            }
            
            return Guid.NewGuid().ToString("N");
        }

        private static string RandomString(int length, System.Random rng)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new System.Text.StringBuilder(length);
            for (int i = 0; i < length; i++) sb.Append(chars[rng.Next(chars.Length)]);
            return sb.ToString();
        }
    }
}
