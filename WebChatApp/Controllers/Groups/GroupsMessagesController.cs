using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using Microsoft.AspNetCore.Authorization;

namespace WebChatApp.Controllers.Groups
{

    [Authorize]
    public class GroupsMessagesController : Controller
    {
        private readonly IGroupsMessagesService _groupsMessagesService;
        private readonly IUserContextService _userContextService;

        public GroupsMessagesController(IGroupsMessagesService groupsMessagesService, IUserContextService userContextService)
        {
            _groupsMessagesService = groupsMessagesService;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessageToGroup(int groupId, [FromBody] string messageContent)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupsMessagesService.AddMessageToGroup(groupId, messageContent, userId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> EditMessageInGroup(int groupId, int messageId, [FromBody] string newContent)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupsMessagesService.EditMessageInGroup(groupId, messageId, newContent, userId);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessageFromGroup(int groupId, int messageId)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupsMessagesService.DeleteMessageFromGroup(groupId, messageId, userId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesFromGroup(int groupId)
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupsMessagesService.GetMessagesFromGroupAsync(groupId, userId);
            return Ok(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetGroupsWithLastMessages()
        {
            var userId = _userContextService.GetCurrentUserId();
            var result = await _groupsMessagesService.GetGroupsWithLastMessages(userId);
            return Ok(result);
        }
    }
}
