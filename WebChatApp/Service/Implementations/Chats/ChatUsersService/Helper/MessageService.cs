    using ApiOpWebE_C.OperationResults;
    using Data_Access_Layer.ProjectRoot.Core.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using WebChatApp.DTO;
    using WebChatApp.Models;
    using WebChatApp.Service.Implementations.CryptographyService;
    using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
    using WebChatApp.Service.Interfaces.iLoggingService;
using WebChatApp.Service.Interfaces.NewFolder;






    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICryptographyService _messageHybrid;
        private readonly ILoggingService _loggingService;

        public MessageService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            IUserContextService userContextService, IConfiguration configuration, ICryptographyService messageHybrid , ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _messageHybrid = messageHybrid;
            _loggingService = loggingService;
        }


        #region CRUDMessage
        public async Task<CreationResult<TbSendMessage>> SendMessageAsync(string fromUserId ,string toUserId, string encryptedMessage)
        {
            var result = new CreationResult<TbSendMessage>();
            try
            {
                if (string.IsNullOrEmpty(fromUserId))
                {
                    result.IsSuccess = false;
                    result.Message = "User not authenticated.";
                    return result;
                }


                var newMessage = new TbSendMessage
                {
                    IdPr = Guid.NewGuid().ToString(),
                    FromUserId = fromUserId,
                    ToUserId = toUserId,
                    EncryptedMessage =  _messageHybrid.EncryptMessageHybridAsync(encryptedMessage).ToString(),
                    WasRead = false,
                    ConnectedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<TbSendMessage>().AddAsync(newMessage);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Message = "Message sent successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(SendMessageAsync));
                result.IsSuccess = false;
                result.Message = "Error occurred while sending the message.";
            }
            return result;
        }
        public async Task<CreationResult<bool>> DeleteMessageAsync(string messageId)
        {
            var result = new CreationResult<bool>();
            try
            {
                var message = await _unitOfWork.Repository<TbSendMessage>().FindAsync(m => m.IdPr == messageId);
                if (message == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Message not found.";
                    return result;
                }

                _unitOfWork.Repository<TbSendMessage>().Delete(message);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Context = true;
                result.Message = "Message deleted successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(DeleteMessageAsync));
                result.IsSuccess = false;
                result.Message = "Error occurred while deleting the message.";
            }

            return result;
        }
        public async Task<CreationResult<TbSendMessage>> UpdateMessageAsync(string messageId, string newEncryptedMessage)
        {
            var result = new CreationResult<TbSendMessage>();
            try
            {
                var message = await _unitOfWork.Repository<TbSendMessage>().FindAsync(m => m.IdPr == messageId);
                if (message == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Message not found.";
                    return result;
                }

                message.EncryptedMessage = _messageHybrid.EncryptMessageHybridAsync(newEncryptedMessage).ToString();
                message.ConnectedAt = DateTime.UtcNow; // تحديث وقت التعديل لو حابب

                _unitOfWork.Repository<TbSendMessage>().Update(message);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Context = message;
                result.Message = "Message updated successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(UpdateMessageAsync));
                result.IsSuccess = false;
                result.Message = "Error occurred while updating the message.";
            }

            return result;
        }
        public async Task<CreationResult<List<TbSendMessage>>> GetMessagesAsync(string user1, string user2)
        {
            await MarkMessagesAsReadAsync(user1, user2);
            return await GetMessagesBetweenUsersAsync(user1, user2);
        }

    #endregion

    public async Task<CreationResult<List<TbSendMessage>>> GetMessagesBetweenUsersAsync(string user1, string user2,  bool onlyUnread = false)
    {
        var result = new CreationResult<List<TbSendMessage>>();
        try
        {
            if (string.IsNullOrEmpty(user1))
            {
                result.IsSuccess = false;
                result.Message = "User not authenticated.";
                return result;
            }

            var messages = await _unitOfWork.Repository<TbSendMessage>().FindAllAsync(msg =>
                ((msg.FromUserId == user1 && msg.ToUserId == user2) ||
                 (msg.FromUserId == user2 && msg.ToUserId == user1)) &&
                 (!onlyUnread || msg.WasRead == false));

            foreach (var message in messages)
            {
                if (!string.IsNullOrEmpty(message.EncryptedMessage))
                {
                    var decryptedMessage = await _messageHybrid.DecryptMessageWithRsaAsync(message.EncryptedMessage);
                    message.EncryptedMessage = decryptedMessage;
                }
            }

            result.IsSuccess = true;
            result.Context = messages.OrderBy(msg => msg.ConnectedAt).ToList();
            result.Message = "Messages retrieved and decrypted successfully.";
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetMessagesBetweenUsersAsync));
            result.IsSuccess = false;
            result.Message = "Error occurred while retrieving messages.";
        }

        return result;
    }

    public async Task<CreationResult<List<TbSendMessage>>> GetUnreadMessagesAsync(string user1,string user2)
        {
            return await GetMessagesBetweenUsersAsync(user1,user2, onlyUnread: true);
        }


        public async Task<CreationResult<List<string>>> GetChatUsersIDsAsync(string username)
        {
            var result = new CreationResult<List<string>>();
            try
            {
                var messages = await _unitOfWork.Repository<TbSendMessage>()
                    .FindAllAsync(msg => msg.FromUserId == username || msg.ToUserId == username);

                var chatUserIds = messages
                    .Where(m => m.FromUserId != username).Select(m => m.FromUserId)
                    .Union(messages.Where(m => m.ToUserId != username).Select(m => m.ToUserId))
                    .Distinct()
                    .ToList();

                result.IsSuccess = true;
                result.Context = chatUserIds;
                result.Message = "Chat users retrieved successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetChatUsersIDsAsync));
                result.IsSuccess = false;
                result.Errors.Add(ex.Message);
                result.Message = "Error occurred while retrieving chat users.";
            }

            return result;
        }
        public async Task<CreationResult<List<UserChatInfo>>> GetUserChatListAsync(string currentUsername)
        {
            var result = new CreationResult<List<UserChatInfo>>();

            try
            {
                var chatUserIdsResult = await GetChatUsersIDsAsync(currentUsername);
                if (!chatUserIdsResult.IsSuccess || chatUserIdsResult.Context == null || !chatUserIdsResult.Context.Any())
                {
                    result.IsSuccess = false;
                    result.Message = "No chat users found.";
                    return result;
                }

                var chatUserIds = chatUserIdsResult.Context;
                var chatList = new List<UserChatInfo>();

                foreach (var userId in chatUserIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var User = GetLastMessageAsync(currentUsername, userId).Result;

                        chatList.Add(new UserChatInfo
                        {
                            UserId = userId,
                            Username = user.UserName,
                            DisplayName = user.UserName,
                            ProfileImage = user.ProfileImage,
                            LastMessageContent = User.Context.EncryptedMessage,
                            LastMessageTime = User.Context.ConnectedAt,
                        });
                    }
                }

                result.IsSuccess = true;
                result.Context = chatList.OrderByDescending(u => u.LastMessageTime).ToList();
                result.Message = "Chat list retrieved successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetUserChatListAsync));
                result.IsSuccess = false;
                result.Message = "Error occurred while retrieving the chat list.";
            }

            return result;
        }




        public async Task<bool> MarkMessagesAsReadAsync(string user1,string user2)
        {
            try
            {

                var unreadMessages = await _unitOfWork.Repository<TbSendMessage>()
                    .FindAllAsync(msg =>
                        ((msg.FromUserId == user1 && msg.ToUserId == user2) ||
                         (msg.FromUserId == user2 && msg.ToUserId == user1)) && !msg.WasRead);

                foreach (var message in unreadMessages)
                {
                    message.WasRead = true;
                }

                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(MarkMessagesAsReadAsync));
           
            }

            return false;
        }

        public async Task<CreationResult<List<UserChatInfo>>> SearchChatUsersByUsernameAsync(string searchTerm, string currentUserId)
        {
            var result = new CreationResult<List<UserChatInfo>>();
            try
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    result.IsSuccess = false;
                    result.Message = "Search term is required.";
                    return result;
                }

                var users = await _unitOfWork.Repository<ApplicationUser>()
                    .GetAllAsync(u => u.UserName.Contains(searchTerm) && u.Id != currentUserId);

                var userChatInfos = new List<UserChatInfo>();

                foreach (var user in users)
                {
                    var unreadMessagesResult = await GetUnreadMessageCountAsync(currentUserId, user.Id);
                    var lastMessageResult = await GetLastMessageAsync(currentUserId, user.Id);

                    userChatInfos.Add(new UserChatInfo
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        DisplayName = user.UserName,
                        ProfileImage = user.ProfileImage,
                        NumberMessage = unreadMessagesResult.Context,
                        LastMessageContent = lastMessageResult.Context?.EncryptedMessage,
                        LastMessageTime = lastMessageResult.Context.ConnectedAt
                    });
                }

                result.IsSuccess = true;
                result.Context = userChatInfos.OrderByDescending(u => u.LastMessageTime).ToList();
                result.Message = "Search results retrieved successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(SearchChatUsersByUsernameAsync));
                result.IsSuccess = false;
                result.Message = "Error occurred while searching for users.";
            }

            return result;
        }


        public async Task<CreationResult<TbSendMessage>> GetLastMessageAsync(string user1, string user2)
        {
            var result = new CreationResult<TbSendMessage>();
            try
            {

                var messages = await _unitOfWork.Repository<TbSendMessage>()
                    .FindAllAsync(msg =>
                        (msg.FromUserId == user1 && msg.ToUserId == user2) ||
                        (msg.FromUserId == user2 && msg.ToUserId == user1));

                var lastMessageEN = messages.OrderByDescending(m => m.ConnectedAt).FirstOrDefault();
            var lastMessageDE = _messageHybrid.DecryptMessageWithRsaAsync(lastMessageEN.EncryptedMessage).ToString();
                result.IsSuccess = true;
                result.Context.EncryptedMessage = lastMessageDE;
                result.Message = "Last message retrieved successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetLastMessageAsync));
                result.IsSuccess = false;
                result.Message = "Error retrieving last message.";
            }

            return result;
        }

    public async Task<CreationResult<int>> GetUnreadMessageCountAsync(string user1, string user2)
    {
        var unreadMessages = await _unitOfWork.Repository<TbSendMessage>()
            .FindAllAsync(msg =>
                (msg.FromUserId == user1 && msg.ToUserId == user2 && !msg.WasRead) ||
                (msg.FromUserId == user2 && msg.ToUserId == user1 && !msg.WasRead));

        var unreadMessageCount = unreadMessages.Count;

        return new CreationResult<int>
        {
            IsSuccess = true,
            Context = unreadMessageCount,
            Message = "Unread message count retrieved successfully."
        };
    }


}
