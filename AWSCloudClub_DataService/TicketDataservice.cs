using MySql.Data.MySqlClient;
using AWSCloudClub_Common.Models;
using AWSCloudClubEventManagement.Data;

namespace AWSCloudClubEventManagement.Services
{
    public class TicketDataService
    {
        private readonly DatabaseHelper _databaseHelper;

        public TicketDataService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        public async Task<int> GetTicketCountForEventAsync(string eventId)
        {
            int ticketCount = 0;
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "SELECT COUNT(*) FROM Tickets WHERE EventID = @EventID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    ticketCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
            }
            return ticketCount;
        }

        public async Task AddTicketAsync(Tickets ticket)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Tickets (TicketID, TicketCode, EventID, MemberID, DateRegistered) VALUES (@TicketID, @TicketCode, @EventID, @MemberID, @DateRegistered)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketID", ticket.TicketID);
                    cmd.Parameters.AddWithValue("@TicketCode", ticket.TicketCode);
                    cmd.Parameters.AddWithValue("@EventID", ticket.EventID);
                    cmd.Parameters.AddWithValue("@MemberID", ticket.MemberID);
                    cmd.Parameters.AddWithValue("@DateRegistered", ticket.DateRegistered);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task RemoveTicketAsync(string ticketId)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "DELETE FROM Tickets WHERE TicketID = @TicketID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketID", ticketId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Tickets?> GetTicketByIDAsync(string ticketID)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM Tickets WHERE TicketID = @TicketID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketID", ticketID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Tickets
                            {
                                TicketID = reader.GetString(reader.GetOrdinal("TicketID")),
                                TicketCode = reader.GetString(reader.GetOrdinal("TicketCode")),
                                EventID = reader.GetString(reader.GetOrdinal("EventID")),
                                MemberID = reader.GetString(reader.GetOrdinal("MemberID")),
                                DateRegistered = reader.GetDateTime(reader.GetOrdinal("DateRegistered"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<List<Tickets>> GetAllTicketsAsync()
        {
            var tickets = new List<Tickets>();
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM Tickets";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tickets.Add(new Tickets
                            {
                                TicketID = reader.GetString(reader.GetOrdinal("TicketID")),
                                TicketCode = reader.GetString(reader.GetOrdinal("TicketCode")),
                                EventID = reader.GetString(reader.GetOrdinal("EventID")),
                                MemberID = reader.GetString(reader.GetOrdinal("MemberID")),
                                DateRegistered = reader.GetDateTime(reader.GetOrdinal("DateRegistered"))
                            });
                        }
                    }
                }
            }
            return tickets;
        }

        public async Task<List<Tickets>> GetTicketsByEventAsync(string EventID)
        {
            var tickets = new List<Tickets>();
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM Tickets WHERE EventID = @EventID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", EventID);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tickets.Add(new Tickets
                            {
                                TicketID = reader.GetString(reader.GetOrdinal("TicketID")),
                                TicketCode = reader.GetString(reader.GetOrdinal("TicketCode")),
                                EventID = reader.GetString(reader.GetOrdinal("EventID")),
                                MemberID = reader.GetString(reader.GetOrdinal("MemberID")),
                                DateRegistered = reader.GetDateTime(reader.GetOrdinal("DateRegistered"))
                            });
                        }
                    }
                }
            }
            return tickets;
        }
        
    }
}