using Microsoft.AspNetCore.Mvc;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Models;
using System.Threading.Tasks;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using WebChatApp.Service.Interfaces.IChats.IGetAllChats;

namespace WebChatApp.Controllers.MainScreen
{
    public class ChatsController : Controller
    {
        private readonly IGetAllChats _groupsService;
        private readonly IUserContextService _userContextService;

        public ChatsController(IGetAllChats groupsService, IUserContextService userContextService)
        {
            _groupsService = groupsService;
            _userContextService = userContextService;
        }

        public async Task<IActionResult> GetChats()
        {
            var currentUsername = _userContextService.GetCurrentUserId();
            var result = await _groupsService.GetAllChatsAsync(currentUsername);

            if (result.IsSuccess)
            {
                return Ok(result);

            }
            else
            {
                ViewBag.ErrorMessage = result.Message; 
            }

            return BadRequest();

        }
    }
}
