using Microsoft.AspNetCore.Mvc;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Models;
using System.Threading.Tasks;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using Microsoft.AspNetCore.Authorization;

namespace WebChatApp.Controllers.Groups
{
    [Authorize]

    public class GroupsController : ControllerBase
    {
        private readonly IGroupsUserManagement _groupsService;
        private readonly IUserContextService _userContextService;

        public GroupsController(IGroupsUserManagement groupsService, IUserContextService userContextService)
        {
            _groupsService = groupsService;
            _userContextService = userContextService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateGroup([FromBody] string groupName)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupsService.CreateGroup(groupName, userId);

            if (result.IsSuccess)
            {
                return Ok(result);  
            }
            else
            {
                return BadRequest(result);  
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateGroupName(int groupId, string newGroupName)
        {
            var result = await _groupsService.UpdateGroupName(groupId, newGroupName);

            if (result.IsSuccess)
            {
                return Ok(result);  
            }
            else
            {
                return BadRequest(result);  
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var result = await _groupsService.DeleteGroup(groupId);

            if (result.IsSuccess)
            {
                return Ok(result); 
            }
            else
            {
                return BadRequest(result);  
            }
        }
    }
}
