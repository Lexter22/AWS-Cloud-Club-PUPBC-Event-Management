using MySql.Data.MySqlClient;
using AWSCloudClub_Common.Models;
using AWSCloudClubEventManagement.Data;

namespace AWSCloudClubEventManagement.Services
{
    public class EventDataService
    {
        private readonly DatabaseHelper _databaseHelper;

        public EventDataService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<Events> GetAllEvents()
        {
            var events = new List<Events>();
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Events";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var eventItem = new Events
                            {
                                EventID = reader.GetString(reader.GetOrdinal("EventID")),
                                EventName = reader.GetString(reader.GetOrdinal("EventName")),
                                EventDate = reader.GetString(reader.GetOrdinal("EventDate")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            };
                            events.Add(eventItem);
                        }
                    }
                }
            }
            return events;
        }
        
        public async Task<List<Events>> GetAllEventsAsync()
        {
            var events = new List<Events>();
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM Events";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var eventItem = new Events
                            {
                                EventID = reader.GetString(reader.GetOrdinal("EventID")),
                                EventName = reader.GetString(reader.GetOrdinal("EventName")),
                                EventDate = reader.GetString(reader.GetOrdinal("EventDate")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            };
                            events.Add(eventItem);
                        }
                    }
                }
            }
            return events;
        }
        public void AddEvent(Events ev)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "INSERT INTO Events (EventID, EventName, EventDate, Description) VALUES (@EventID, @EventName, @EventDate, @Description)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", ev.EventID);
                    cmd.Parameters.AddWithValue("@EventName", ev.EventName);
                    cmd.Parameters.AddWithValue("@EventDate", ev.EventDate);
                    cmd.Parameters.AddWithValue("@Description", ev.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task AddEventAsync(Events ev)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "INSERT INTO Events (EventID, EventName, EventDate, Description) VALUES (@EventID, @EventName, @EventDate, @Description)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", ev.EventID);
                    cmd.Parameters.AddWithValue("@EventName", ev.EventName);
                    cmd.Parameters.AddWithValue("@EventDate", ev.EventDate);
                    cmd.Parameters.AddWithValue("@Description", ev.Description);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public void RemoveEvent(string eventId)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "DELETE FROM Events WHERE EventID = @EventID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateEvent(Events ev)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "UPDATE Events SET EventName = @EventName, EventDate = @EventDate, Description = @Description WHERE EventID = @EventID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventName", ev.EventName);
                    cmd.Parameters.AddWithValue("@EventDate", ev.EventDate);
                    cmd.Parameters.AddWithValue("@Description", ev.Description);
                    cmd.Parameters.AddWithValue("@EventID", ev.EventID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<bool> UpdateEventAsync(string eventId, Events ev)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                await conn.OpenAsync();
                var query = "UPDATE Events SET EventName = @EventName, EventDate = @EventDate, Description = @Description WHERE EventID = @EventID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventName", ev.EventName);
                    cmd.Parameters.AddWithValue("@EventDate", ev.EventDate);
                    cmd.Parameters.AddWithValue("@Description", ev.Description);
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    var rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }
        public List<Events> SearchEventByID(string EventID)
        {
            var events = new List<Events>();
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Events WHERE EventID = @EventID";
                
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EventID", EventID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var eventItem = new Events
                            {
                                EventID = reader.GetString(reader.GetOrdinal("EventID")),
                                EventName = reader.GetString(reader.GetOrdinal("EventName")),
                                EventDate = reader.GetString(reader.GetOrdinal("EventDate")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            };
                            events.Add(eventItem);
                        }
                    }
                }
            }
            return events;
        }
    }
}