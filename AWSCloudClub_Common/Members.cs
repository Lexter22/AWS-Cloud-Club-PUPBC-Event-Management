namespace AWSCloudClub_Common.Models
{
    public class Members
    {
        public required string MemberID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string City { get; set; }
        public required string MembersPassword { get; set; }
    }
}