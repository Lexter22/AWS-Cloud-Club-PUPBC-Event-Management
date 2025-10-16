using AWSCloudClub_Common.Models;
using AWSCloudClubEventManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSCloudClub_BusinessLogic
{
    public class TicketBusinessLogic : ITicketBusinessLogic
    {
        private readonly TicketDataService _dataService;

        public TicketBusinessLogic(TicketDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<(bool Success, string? Error)> AddTicketAsync(Tickets ticket)
        {
            if (ticket == null) return (false, "Invalid ticket payload");
            if (string.IsNullOrWhiteSpace(ticket.TicketID) || string.IsNullOrWhiteSpace(ticket.TicketCode))
                return (false, "TicketID and TicketCode are required");

            // Check for duplicate TicketID
            var existingById = await _dataService.GetTicketByIDAsync(ticket.TicketID);
            if (existingById != null) return (false, "DuplicateTicketID");

            // Check for duplicate TicketCode across all tickets for the same event
            var ticketsForEvent = await _dataService.GetTicketsByEventAsync(ticket.EventID);
            if (ticketsForEvent != null)
            {
                foreach (var t in ticketsForEvent)
                {
                    if (t.TicketCode == ticket.TicketCode) return (false, "DuplicateTicketCode");
                }
            }

            await _dataService.AddTicketAsync(ticket);
            return (true, null);
        }

        public async Task<bool> RemoveTicketAsync(string ticketId)
        {
            if (string.IsNullOrWhiteSpace(ticketId)) return false;
            await _dataService.RemoveTicketAsync(ticketId);
            return true;
        }

        public Task<Tickets?> GetTicketByIdAsync(string ticketId) => _dataService.GetTicketByIDAsync(ticketId);

        public Task<List<Tickets>> GetAllTicketsAsync() => _dataService.GetAllTicketsAsync();

        public Task<List<Tickets>> GetTicketsByEventAsync(string eventId) => _dataService.GetTicketsByEventAsync(eventId);
    }
}
