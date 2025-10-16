using AWSCloudClubEventManagement.Data;
using Microsoft.AspNetCore.Mvc;
namespace AWSCloudClubEventManagement.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class DBConnectionController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public DBConnectionController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

    [HttpGet("test")]
    public IActionResult TestConnection()
        {
            bool isConnected = _databaseHelper.TestConnection();
            if (isConnected)
            {
                return Ok("Database connection successful.");
            }
            else
            {
                return StatusCode(500, "Database connection failed.");
            }
        }
    }
}