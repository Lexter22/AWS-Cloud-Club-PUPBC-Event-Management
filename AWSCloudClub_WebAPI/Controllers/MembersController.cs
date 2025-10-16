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
            if (member == null ||
                string.IsNullOrEmpty(member.MemberID) ||
                string.IsNullOrEmpty(member.FirstName) ||
                string.IsNullOrEmpty(member.LastName) ||
                string.IsNullOrEmpty(member.Email) ||
                string.IsNullOrEmpty(member.MembersPassword))
            {
                return BadRequest("Invalid member data!");
            }

            bool isAdded = _memberBusiness.AddMember(member);
            if (isAdded)
            {
                return Ok("Member added successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to add member!");
            }
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
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required!");
            }

            var member = _memberBusiness.GetMemberByEmail(email);
            if (member != null)
            {
                return Ok(member);
            }
            return NotFound("Member not found!");
        }
        [HttpDelete("delete/{memberID}")]
        public IActionResult RemoveMember(string memberID)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return BadRequest("MemberID is required!");
            }

            _memberBusiness.RemoveMember(memberID);
            return Ok("Member removed successfully!");
        }
        [HttpPost("ChangePassword/{memberID}")]
        public IActionResult ChangePassword(string memberID, [FromBody] string newPassword)
        {
            if (string.IsNullOrEmpty(memberID) || string.IsNullOrEmpty(newPassword))
            {
                return BadRequest("MemberID and new password are required!");
            }

            var member = _memberBusiness.GetMemberByEmail(memberID);
            if (member == null)
            {
                return NotFound("Member not found!");
            }

            _memberBusiness.ChangePassword(member.Email, newPassword);
            return Ok("Password changed successfully.");
        }
    }
}