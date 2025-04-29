using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;

namespace WebChatApp.Controllers.Groups
{

    [Authorize]

    public class GroupMemberController : Controller
    {
        private readonly IGroupMemberService _groupMemberService;
        private readonly IUserContextService _userContextService;



        public GroupMemberController(IGroupMemberService groupMemberService, IUserContextService userContextService)
        {
            _groupMemberService = groupMemberService;
            _userContextService = userContextService;

        }

        [HttpPost]
        public async Task<IActionResult> AddUserToGroup(int groupId, string userId)
        {

            var result = await _groupMemberService.AddUserToGroup(groupId, userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromGroup(int groupId, string userId)
        {
            var result = await _groupMemberService.RemoveUserFromGroup(groupId, userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetMembersOfGroup(int groupId)
        {
            var result = await _groupMemberService.GetMembersOfGroup(groupId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetGroupMembersCount(int groupId)
        {
            var result = await _groupMemberService.GetGroupMembersCount(groupId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpPost]
        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupMemberService.LeaveGroup(groupId, userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> JoinGroup(int groupId)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupMemberService.JoinGroup(groupId, userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> PromoteUserToAdmin(int groupId, string userId)
        {
            var result = await _groupMemberService.PromoteUserToAdmin(groupId, userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> DemoteAdminToUser(int groupId, string userId)
        {
            var result = await _groupMemberService.DemoteAdminToUser(groupId, userId);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }



           }

}

