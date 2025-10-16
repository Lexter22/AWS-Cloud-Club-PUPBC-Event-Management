using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AWSCloudClubEventManagement.Data
{
    public class DatabaseHelper
    {
        private readonly string? _connectionString;
        private readonly ILogger<DatabaseHelper>? _logger;

        public DatabaseHelper(IConfiguration configuration, ILogger<DatabaseHelper>? logger = null)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString ?? string.Empty);
        }

        public bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Database connection test failed. ConnectionString: {ConnectionString}", _connectionString ?? "<null>");
                return false;
            }
        }
    }
}
