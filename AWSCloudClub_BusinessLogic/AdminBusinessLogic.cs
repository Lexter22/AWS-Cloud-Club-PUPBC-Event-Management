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

        public (bool Success, string ErrorMessage) CreateAdmin(Admins admin)
        {
            // Validation
            if (admin == null)
                return (false, "Admin data is required.");
            
            if (string.IsNullOrWhiteSpace(admin.AdminID))
                return (false, "AdminID is required.");
            
            if (string.IsNullOrWhiteSpace(admin.FirstName))
                return (false, "First name is required.");
            
            if (string.IsNullOrWhiteSpace(admin.LastName))
                return (false, "Last name is required.");
            
            if (string.IsNullOrWhiteSpace(admin.Email))
                return (false, "Email is required.");
            
            if (string.IsNullOrWhiteSpace(admin.AdminsPassword))
                return (false, "Password is required.");
            
            if (string.IsNullOrWhiteSpace(admin.Role))
                return (false, "Role is required.");

            // Business logic
            admin.AdminsPassword = HashPassword(admin.AdminsPassword);
            bool success = _dataService.AddAdmin(admin);
            
            return success ? (true, string.Empty) : (false, "Failed to create admin.");
        }

        public List<Admins> GetAllAdmins()
        {
            return _dataService.GetAllAdmins();
        }

        public void RemoveAdmin(string adminId) => _dataService.RemoveAdmin(adminId);

        public (List<Admins>? Admins, string ErrorMessage) GetAdminByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return (null, "Email is required.");
            
            var admins = _dataService.GetAdminByEmail(email);
            return (admins, string.Empty);
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
