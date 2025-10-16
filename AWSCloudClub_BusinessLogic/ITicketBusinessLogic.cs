using AWSCloudClub_Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSCloudClub_BusinessLogic
{
    public interface ITicketBusinessLogic
    {
        Task<(bool Success, string? Error)> AddTicketAsync(Tickets ticket);
        Task<bool> RemoveTicketAsync(string ticketId);
        Task<Tickets?> GetTicketByIdAsync(string ticketId);
        Task<List<Tickets>> GetAllTicketsAsync();
        Task<List<Tickets>> GetTicketsByEventAsync(string eventId);
    }
}
