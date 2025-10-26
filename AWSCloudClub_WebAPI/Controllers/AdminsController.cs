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
            var (success, errorMessage) = _adminBusiness.CreateAdmin(admin);
            
            if (success)
                return Ok("Admin added successfully.");
            
            return BadRequest(errorMessage);
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
            var (admins, errorMessage) = _adminBusiness.GetAdminByEmail(email);
            
            if (!string.IsNullOrEmpty(errorMessage))
                return BadRequest(errorMessage);
            
            if (admins == null || admins.Count == 0)
                return NotFound("Admin not found!");
            
            return Ok(admins);
        }
    }
}