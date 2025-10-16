namespace AWSCloudClub_Common.Models
{
    public class Admins
    {
        public required string AdminID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string AdminsPassword { get; set; }
        public required string Role { get; set; }
    }
}