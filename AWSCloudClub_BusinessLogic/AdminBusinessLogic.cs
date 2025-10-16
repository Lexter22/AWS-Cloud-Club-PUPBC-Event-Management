using AWSCloudClub_Common.Models;
using AWSCloudClub_DataService;
using System.Security.Cryptography;
using System.Text;

namespace AWSCloudClub_BusinessLogic
{
    public class AdminBusinessLogic
    {
        private readonly AdminDataService _dataService;

        public AdminBusinessLogic(AdminDataService dataService)
        {
            _dataService = dataService;
        }

        public bool CreateAdmin(Admins admin)
        {
            if (admin == null) return false;
            if (string.IsNullOrWhiteSpace(admin.AdminID) || string.IsNullOrWhiteSpace(admin.Email)) return false;

            admin.AdminsPassword = HashPassword(admin.AdminsPassword);
            return _dataService.AddAdmin(admin);
        }

        public List<Admins> GetAllAdmins()
        {
            return _dataService.GetAllAdmins();
        }

        public void RemoveAdmin(string adminId) => _dataService.RemoveAdmin(adminId);

        public List<Admins> GetAdminByEmail(string email)
        {
            return _dataService.GetAdminByEmail(email);
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
