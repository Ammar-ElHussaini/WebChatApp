using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebChatApp.Service.Interfaces;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper; 

namespace WebChatApp.Controllers.ChatsUser
{

    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserContextService _userContextService;

        public MessageController(IMessageService messageService, IUserContextService userContextService)
        {
            _messageService = messageService;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string toUserId, string encryptedMessage)
        {
            var fromUserId = _userContextService.GetCurrentUserId();
            var result = await _messageService.SendMessageAsync(fromUserId, toUserId, encryptedMessage);
            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessage(string messageId, string newEncryptedMessage)
        {
            var result = await _messageService.UpdateMessageAsync(messageId, newEncryptedMessage);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(string messageId)
        {
            var result = await _messageService.DeleteMessageAsync(messageId);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesBetweenUsers(string userId)
        {
            var currentUserId = _userContextService.GetCurrentUserId();
            var result = await _messageService.GetMessagesAsync(currentUserId, userId);
            return Json(result);
        }



    }
}
