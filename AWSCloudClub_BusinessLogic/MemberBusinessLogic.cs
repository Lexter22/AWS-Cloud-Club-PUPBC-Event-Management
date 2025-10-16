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

        public bool AddMember(Members member)
        {
            if (member == null) return false;
            if (string.IsNullOrWhiteSpace(member.MemberID) || string.IsNullOrWhiteSpace(member.Email)) return false;

            member.MembersPassword = HashPassword(member.MembersPassword);
            return _dataService.AddMember(member);
        }

        public List<Members> GetAllMembers() => _dataService.GetAllMembers();

        public Members? GetMemberByEmail(string email) => _dataService.GetMemberByEmail(email);

        public void RemoveMember(string memberId) => _dataService.RemoveMember(memberId);

        public void ChangePassword(string email, string newPassword)
        {
            var hashed = HashPassword(newPassword);
            _dataService.ChangeMemberPassword(email, hashed);
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
