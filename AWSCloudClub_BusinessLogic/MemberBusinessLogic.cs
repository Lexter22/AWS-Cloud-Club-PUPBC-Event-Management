using AWSCloudClub_Common.Models;
using AWSCloudClub_DataService;
using System.Security.Cryptography;
using System.Text;

namespace AWSCloudClub_BusinessLogic
{
    public class MemberBusinessLogic
    {
        private readonly MemberDataService _dataService;

        public MemberBusinessLogic(MemberDataService dataService)
        {
            _dataService = dataService;
        }

        public (bool Success, string ErrorMessage) AddMember(Members member)
        {
            if (member == null)
                return (false, "Member data is required.");
            
            if (string.IsNullOrWhiteSpace(member.MemberID))
                return (false, "MemberID is required.");
            
            if (string.IsNullOrWhiteSpace(member.FirstName))
                return (false, "First name is required.");
            
            if (string.IsNullOrWhiteSpace(member.LastName))
                return (false, "Last name is required.");
            
            if (string.IsNullOrWhiteSpace(member.Email))
                return (false, "Email is required.");
            
            if (string.IsNullOrWhiteSpace(member.MembersPassword))
                return (false, "Password is required.");

            member.MembersPassword = HashPassword(member.MembersPassword);
            bool success = _dataService.AddMember(member);
            
            return success ? (true, string.Empty) : (false, "Failed to add member.");
        }

        public List<Members> GetAllMembers() => _dataService.GetAllMembers();

        public (Members? Member, string ErrorMessage) GetMemberByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (null, "Email is required.");
            
            var member = _dataService.GetMemberByEmail(email);
            return (member, string.Empty);
        }

        public (bool Success, string ErrorMessage) RemoveMember(string memberId)
        {
            if (string.IsNullOrWhiteSpace(memberId))
                return (false, "MemberID is required.");
            
            _dataService.RemoveMember(memberId);
            return (true, string.Empty);
        }

        public (bool Success, string ErrorMessage) ChangePassword(string email, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (false, "Email is required.");
            
            if (string.IsNullOrWhiteSpace(newPassword))
                return (false, "New password is required.");

            var hashed = HashPassword(newPassword);
            _dataService.ChangeMemberPassword(email, hashed);
            return (true, string.Empty);
        }

        private static string HashPassword(string password)
        {
            if (password == null) return string.Empty;
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
