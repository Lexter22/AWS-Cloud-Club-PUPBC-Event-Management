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

        public string CreateEvent(Events ev)
        {
            if (ev == null) return string.Empty;
            if (string.IsNullOrWhiteSpace(ev.EventID) || string.IsNullOrWhiteSpace(ev.EventName)) return string.Empty;

            // ensure EventID is unique
            var existing = _dataService.SearchEventByID(ev.EventID);
            if (existing != null && existing.Count > 0) return string.Empty;

            _dataService.AddEvent(ev);
            return ev.EventID;
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

        public void RemoveEvent(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId)) return;
            _dataService.RemoveEvent(eventId);
        }

        public void UpdateEvent(Events ev)
        {
            if (ev == null) return;
            if (string.IsNullOrWhiteSpace(ev.EventID)) return; // EventID is immutable
            _dataService.UpdateEvent(ev);
        }

        public Task<bool> UpdateEventAsync(string eventId, Events ev)
        {
            if (string.IsNullOrWhiteSpace(eventId) || ev == null) return Task.FromResult(false);
            return _dataService.UpdateEventAsync(eventId, ev);
        }

        public List<Events> SearchEventByID(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId)) return new List<Events>();
            return _dataService.SearchEventByID(eventId);
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
