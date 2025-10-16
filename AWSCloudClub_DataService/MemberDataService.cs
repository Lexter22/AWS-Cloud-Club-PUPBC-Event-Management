using MySql.Data.MySqlClient;
using AWSCloudClub_Common;
using System.Collections.Generic;
using AWSCloudClub_Common.Models;
using AWSCloudClubEventManagement.Data;

namespace AWSCloudClub_DataService
{
    public class MemberDataService
    {
        private readonly DatabaseHelper _databaseHelper;

        public MemberDataService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public bool AddMember(Members member)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "INSERT INTO Members (MemberID, FirstName, LastName, Email, City, MembersPassword) VALUES (@MemberID, @FirstName, @LastName, @Email, @City, @MembersPassword)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", member.MemberID);
                    cmd.Parameters.AddWithValue("@FirstName", member.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", member.LastName);
                    cmd.Parameters.AddWithValue("@Email", member.Email);
                    cmd.Parameters.AddWithValue("@City", member.City);
                    cmd.Parameters.AddWithValue("@MembersPassword", member.MembersPassword);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
        }
        public List<Members> GetAllMembers()
        {
            var members = new List<Members>();
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Members";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var member = new Members
                            {
                                MemberID = reader.GetString(reader.GetOrdinal("MemberID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                MembersPassword = reader.GetString(reader.GetOrdinal("MembersPassword")),
                            };
                            members.Add(member);
                        }
                    }
                }
            }
            return members;
        }
        public void RemoveMember(string memberId)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "DELETE FROM Members WHERE MemberID = @MemberID";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public Members? GetMemberByEmail(string email)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Members WHERE Email = @Email";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Members
                            {
                                MemberID = reader.GetString(reader.GetOrdinal("MemberID")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                MembersPassword = reader.GetString(reader.GetOrdinal("MembersPassword")),
                            };
                        }
                    }
                }
            }
            return null;
        }
        public void ChangeMemberPassword(string email, string newPassword)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                conn.Open();
                var query = "UPDATE Members SET MembersPassword = @MembersPassword WHERE Email = @Email";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MembersPassword", newPassword);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
                   