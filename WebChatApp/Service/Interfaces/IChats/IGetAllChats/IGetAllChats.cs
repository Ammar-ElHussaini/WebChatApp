using ApiOpWebE_C.OperationResults;
using WebChatApp.Models;

namespace WebChatApp.Service.Interfaces.IChats.IGetAllChats
{
    public interface IGetAllChats
    {
        Task<CreationResult<List<ChatItem>>> GetAllChatsAsync(string currentUsername);
    }
}
