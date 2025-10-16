using MySql.Data.MySqlClient;
using AWSCloudClub_Common;
using System.Collections.Generic;
using AWSCloudClub_Common.Models;
using AWSCloudClubEventManagement.Data;

namespace AWSCloudClub_DataService
{
    public class AdminDataService
    {
        private readonly DatabaseHelper _databaseHelper;

        public AdminDataService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public bool AddAdmin(Admins admin)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "INSERT INTO Admins (AdminID, FirstName, LastName, Email, AdminsPassword, Role) VALUES (@AdminID, @FirstName, @LastName, @Email, @AdminsPassword, @Role)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", admin.AdminID);
                    cmd.Parameters.AddWithValue("@FirstName", admin.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", admin.LastName);
                    cmd.Parameters.AddWithValue("@Email", admin.Email);
                    cmd.Parameters.AddWithValue("@AdminsPassword", admin.AdminsPassword);
                    cmd.Parameters.AddWithValue("@Role", admin.Role);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
        }

        public List<Admins> GetAllAdmins()
        {
            var admins = new List<Admins>();
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Admins";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var admin = new Admins
                            {
                                AdminID = reader.GetString(reader.GetOrdinal("AdminID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                AdminsPassword = reader.GetString(reader.GetOrdinal("AdminsPassword")),
                                Role = reader.GetString(reader.GetOrdinal("Role"))
                            };
                            admins.Add(admin);
                        }
                    }
                }
            }
            return admins;
        }

        public void RemoveAdmin(string adminID)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "DELETE FROM Admins WHERE AdminID = @AdminID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", adminID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Admins> GetAdminByEmail(string email)
        {
            var admins = new List<Admins>();
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Admins WHERE Email = @Email";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var admin = new Admins
                            {
                                AdminID = reader.GetString(reader.GetOrdinal("AdminID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                AdminsPassword = reader.GetString(reader.GetOrdinal("AdminsPassword")),
                                Role = reader.GetString(reader.GetOrdinal("Role"))
                            };
                            admins.Add(admin);
                        }
                    }
                }
            }
            return admins;
        }
    }
}