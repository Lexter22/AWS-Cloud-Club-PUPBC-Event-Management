using Microsoft.AspNetCore.Mvc;
using AWSCloudClub_Common.Models;
using AWSCloudClub_DataService;
using AWSCloudClub_BusinessLogic;

namespace AWSCloudClubEventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly AdminBusinessLogic _adminBusiness;

        public AdminsController(AdminBusinessLogic adminBusiness)
        {
            _adminBusiness = adminBusiness;
        }

        [HttpPost("add")]
        public IActionResult AddAdmin(Admins admin)
        {
            if (admin == null ||
                string.IsNullOrEmpty(admin.AdminID) ||
                string.IsNullOrEmpty(admin.FirstName) ||
                string.IsNullOrEmpty(admin.LastName) ||
                string.IsNullOrEmpty(admin.Email) ||
                string.IsNullOrEmpty(admin.AdminsPassword) ||
                string.IsNullOrEmpty(admin.Role))
            {
                return BadRequest("Invalid admin data.");
            }

            bool isAdded = _adminBusiness.CreateAdmin(admin);
            if (isAdded)
            {
                return Ok("Admin added successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to add admin.");
            }
        }

        [HttpGet("all")]
        public IActionResult GetAllAdmins()
        {
            var admins = _adminBusiness.GetAllAdmins();
            return Ok(admins);
        }
    [HttpGet("search")]
    public IActionResult SearchAdminByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email parameter is required.");
            }

            var admin = _adminBusiness.GetAdminByEmail(email);
            if (admin != null)
            {
                return Ok(admin);
            }
            else
            {
                return NotFound("Admin not found!");
            }
        }
    }
}