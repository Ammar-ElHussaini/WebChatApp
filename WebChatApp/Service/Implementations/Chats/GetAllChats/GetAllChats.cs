using ApiOpWebE_C.OperationResults;
using Microsoft.AspNetCore.Identity;
using WebChatApp.Models;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using WebChatApp.Service.Interfaces.IChats.IGetAllChats;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Service.Interfaces.iLoggingService;

namespace WebChatApp.Service.Implementations.Chats.GetAllChats
{
    public class GetAllChats(IMessageService _messageService, IGroupsMessagesService
         _groupsMessages, UserManager<ApplicationUser> _userManager, ILoggingService _loggingService) : IGetAllChats


    {
        public async Task<CreationResult<List<ChatItem>>> GetAllChatsAsync(string currentUsername)
        {
            var result = new CreationResult<List<ChatItem>>();

            try
            {
                var combinedChats = new List<ChatItem>();

                // Get private chat list
                var privateChatsResult = await _messageService.GetUserChatListAsync(currentUsername);
                if (privateChatsResult.IsSuccess && privateChatsResult.Context != null)
                {
                    var privateChats = privateChatsResult.Context.Select(chat => new ChatItem
                    {
                        Id = chat.UserId,
                        Name = chat.DisplayName,
                        LastMessageContent = chat.LastMessageContent,
                        LastMessageTime = chat.LastMessageTime,
                        IsGroup = false,
                        ProfileImage = chat.ProfileImage
                    }).ToList();

                    combinedChats.AddRange(privateChats);
                }

                // Get group chats list
                var currentUser = await _userManager.FindByNameAsync(currentUsername);
                if (currentUser != null)
                {
                    var groupChatsResult = await _groupsMessages.GetGroupsWithLastMessages(currentUser.Id);
                    if (groupChatsResult.IsSuccess && groupChatsResult.Context != null)
                    {
                        var groupChats = groupChatsResult.Context.Select(group => new ChatItem
                        {
                            Id = group.GroupId.ToString(),
                            Name = group.GroupName,
                            LastMessageContent = group.LastMessageContent,
                            LastMessageTime = group.LastMessageSentAt,
                            IsGroup = true,
                            ProfileImage = null // مفيش صورة جروب دلوقتي
                        }).ToList();

                        combinedChats.AddRange(groupChats);
                    }
                }

                // Sort all chats by last message time descending
                combinedChats = combinedChats
                    .OrderByDescending(c => c.LastMessageTime)
                    .ToList();

                result.IsSuccess = true;
                result.Context = combinedChats;
                result.Message = "Chats retrieved successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetAllChatsAsync));
                result.IsSuccess = false;
                result.Message = "An error occurred while retrieving chats.";
            }

            return result;
        }

    }
}
