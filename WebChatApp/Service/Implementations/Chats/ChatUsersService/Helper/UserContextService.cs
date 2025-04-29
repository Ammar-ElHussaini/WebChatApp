using System.Security.Claims;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;

namespace WebChatApp.Service.Implementations.Chats.ChatUsersService.Helper
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }


    }
}
