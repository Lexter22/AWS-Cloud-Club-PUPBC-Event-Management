namespace AWSCloudClub_Common.Models
{
    public class Tickets
    {
        public required string TicketID { get; set; }
        public required string TicketCode { get; set; }
        public required string EventID { get; set; }
        public required string MemberID { get; set; }
        public DateTime DateRegistered { get; set; }

    }
}