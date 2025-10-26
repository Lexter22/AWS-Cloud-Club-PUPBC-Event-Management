using Microsoft.AspNetCore.Mvc;
using AWSCloudClubEventManagement.Data;
using AWSCloudClub_Common.Models;
using AWSCloudClub_DataService;
using AWSCloudClub_BusinessLogic;

namespace AWSCloudClubEventManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : ControllerBase
    {
        private readonly MemberBusinessLogic _memberBusiness;

        public MembersController(MemberBusinessLogic memberBusiness)
        {
            _memberBusiness = memberBusiness;
        }

        [HttpPost("add")]
        public IActionResult AddMember(Members member)
        {
            var (success, errorMessage) = _memberBusiness.AddMember(member);
            
            if (success)
                return Ok("Member added successfully.");
            
            return BadRequest(errorMessage);
        }

        [HttpGet("GetAllMembers")]
        public IActionResult GetAllMembers()
        {
            var members = _memberBusiness.GetAllMembers();
            return Ok(members);
        }
        [HttpGet("SeacrchMemberByEmail")]
        public IActionResult SearchMemberByEmail(string email)
        {
            var (member, errorMessage) = _memberBusiness.GetMemberByEmail(email);
            
            if (!string.IsNullOrEmpty(errorMessage))
                return BadRequest(errorMessage);
            
            if (member == null)
                return NotFound("Member not found!");
            
            return Ok(member);
        }
        [HttpDelete("delete/{memberID}")]
        public IActionResult RemoveMember(string memberID)
        {
            var (success, errorMessage) = _memberBusiness.RemoveMember(memberID);
            
            if (success)
                return Ok("Member removed successfully!");
            
            return BadRequest(errorMessage);
        }
        
        [HttpPost("ChangePassword/{memberID}")]
        public IActionResult ChangePassword(string memberID, [FromBody] string newPassword)
        {
            var (success, errorMessage) = _memberBusiness.ChangePassword(memberID, newPassword);
            
            if (success)
                return Ok("Password changed successfully.");
            
            return BadRequest(errorMessage);
        }
    }
}