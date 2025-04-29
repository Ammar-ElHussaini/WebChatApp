using ApiOpWebE_C.OperationResults;
using WebChatApp.DTO;
using WebChatApp.Models;

namespace WebChatApp.Service.Interfaces.IChats.IChatService.Helper
{
    public interface IMessageService
    {
        Task<CreationResult<TbSendMessage>> SendMessageAsync(string fromUserId, string toUserId, string encryptedMessage);

        Task<CreationResult<TbSendMessage>> UpdateMessageAsync(string messageId, string newEncryptedMessage);

     
        Task<CreationResult<bool>> DeleteMessageAsync(string messageId);

        Task<CreationResult<List<TbSendMessage>>> GetMessagesBetweenUsersAsync(string user1, string user2, bool onlyUnread = false);

        Task<CreationResult<List<TbSendMessage>>> GetUnreadMessagesAsync(string user1, string user2);

        Task<CreationResult<List<TbSendMessage>>> GetMessagesAsync(string user1, string user2);

        Task<bool> MarkMessagesAsReadAsync(string user1, string user2);

        Task<CreationResult<List<string>>> GetChatUsersIDsAsync(string username);

        Task<CreationResult<List<UserChatInfo>>> GetUserChatListAsync(string currentUsername);

        Task<CreationResult<List<UserChatInfo>>> SearchChatUsersByUsernameAsync(string searchTerm, string currentUserId);

        Task<CreationResult<TbSendMessage>> GetLastMessageAsync(string user1, string user2);

        Task<CreationResult<int>> GetUnreadMessageCountAsync(string user1, string user2);
    }
}
