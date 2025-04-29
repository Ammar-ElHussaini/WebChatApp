using ApiOpWebE_C.OperationResults;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChatApp.Data_Acess_Layer.Models.DB;
using WebChatApp.Models;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Service.Interfaces.iLoggingService;

namespace WebChatApp.Service.Implementations.Chats.GroupsServices.Helper
{
    public class GroupsMessagesService : IGroupsMessagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggingService _loggingService;

        public GroupsMessagesService(IUnitOfWork unitOfWork, IUserContextService userContextService, ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _loggingService = loggingService;
        }


        #region Crud
        public async Task<CreationResult<TbGroupsMessages>> AddMessageToGroup(int groupId, string messageContent, string userId)
        {
            var result = new CreationResult<TbGroupsMessages>();
            try
            {
                var isAdmin = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == userId && gm.IsAdmin);

                if (isAdmin == null)
                {
                    result.IsSuccess = false;
                    result.Message = "User is not an admin of this group.";
                    return result;
                }

                var message = new TbGroupsMessages
                {
                    GroupId = groupId,
                    FromUser = userId,
                    MessageContent = messageContent,
                    SentAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<TbGroupsMessages>().AddAsync(message);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Message = "Message added successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(AddMessageToGroup));

                result.IsSuccess = false;
                result.Errors.Add(ex.Message);
                result.Message = "An error occurred while adding the message.";
            }
            return result;
        }

        public async Task<CreationResult<TbGroupsMessages>> EditMessageInGroup(int groupId, int messageId, string newContent, string userId)
        {
            var result = new CreationResult<TbGroupsMessages>();
            try
            {
                var message = await _unitOfWork.Repository<TbGroupsMessages>()
                    .FindAsync(m => m.GroupId == groupId && m.MessageId == messageId && m.FromUser == userId);

                if (message == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Message not found or user is not authorized to edit this message.";
                    return result;
                }

                message.MessageContent = newContent;
                message.SentAt = DateTime.UtcNow;

                _unitOfWork.Repository<TbGroupsMessages>().Update(message);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Message = "Message edited successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(EditMessageInGroup));
                result.IsSuccess = false;
                result.Message = "An error occurred while editing the message.";
            }

            return result;
        }

        public async Task<CreationResult<bool>> DeleteMessageFromGroup(int groupId, int messageId, string userId)
        {
            var result = new CreationResult<bool>();
            try
            {
                var message = await _unitOfWork.Repository<TbGroupsMessages>()
                    .FindAsync(m => m.GroupId == groupId && m.MessageId == messageId && m.FromUser == userId);

                if (message == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Message not found or user is not authorized to delete this message.";
                    return result;
                }

                _unitOfWork.Repository<TbGroupsMessages>().Delete(message);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Context = true;
                result.Message = "Message deleted successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(DeleteMessageFromGroup));
                result.IsSuccess = false;
                result.Message = "An error occurred while deleting the message.";
            }

            return result;
        }
        public async Task<CreationResult<List<TbGroupsMessages>>> GetMessagesFromGroupAsync(int groupId, string userId)
        {
            var result = new CreationResult<List<TbGroupsMessages>>();

            try
            {
                var isMember = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

                if (isMember == null)
                {
                    result.IsSuccess = false;
                    result.Message = "User is not a member of this group.";
                    return result;
                }

                var messages = await _unitOfWork.Repository<TbGroupsMessages>()
                    .FindAllAsync(m => m.GroupId == groupId);



                result.IsSuccess = true;
                result.Context = messages.OrderBy(m => m.SentAt).ToList();
                result.Message = "Messages retrieved successfully.";


                var unreadMessages = await _unitOfWork.Repository<TbMessageReadStatusGroups>()
                    .FindAllAsync(m => m.GroupId == groupId && m.ReadAt == null);

                foreach (var message in unreadMessages)
                {
                    message.ReadAt = DateTime.Now;
                    await _unitOfWork.Repository<TbMessageReadStatusGroups>().AddAsync(message);
                }





            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetMessagesFromGroupAsync));
                result.IsSuccess = false;
                result.Message = "An error occurred while retrieving messages from the group.";
            }

            return result;
        }

        #endregion
        public async Task<CreationResult<List<TbGroupsMessages>>> GetUnreadMessagesForUser(int groupId, string userId)
        {
            var result = new CreationResult<List<TbGroupsMessages>>();
            try
            {
                var messages = await _unitOfWork.Repository<TbGroupsMessages>()
                    .FindAllAsync(m => m.GroupId == groupId);

                var readMessages = await _unitOfWork.Repository<TbMessageReadStatusGroups>()
                    .FindAllAsync(mrs => mrs.GroupId == groupId && mrs.UserId == userId && mrs.ReadAt != null);

                var unreadMessages = messages
                    .Where(m => !readMessages.Any(rm => rm.MessageId == m.MessageId))
                    .ToList();

                await MarkMessagesAsRead(unreadMessages, userId, groupId);

                result.IsSuccess = true;
                result.Context = unreadMessages;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetUnreadMessagesForUser));
                result.IsSuccess = false;
                result.Message = "An error occurred while retrieving unread messages.";
            }

            return result;
        }

        private async Task MarkMessagesAsRead(List<TbGroupsMessages> unreadMessages, string userId, int groupId)
        {
            foreach (var message in unreadMessages)
            {
                var messageReadStatus = new TbMessageReadStatusGroups
                {
                    MessageId = message.MessageId,
                    GroupId = groupId,
                    UserId = userId,
                    ReadAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<TbMessageReadStatusGroups>().AddAsync(messageReadStatus);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<CreationResult<List<GroupWithLastMessage>>> GetGroupsWithLastMessages(string userId)
        {
            var result = new CreationResult<List<GroupWithLastMessage>>();
            try
            {
                var userGroups = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAllAsync(gm => gm.UserId == userId);

                var groupIds = userGroups.Select(g => g.GroupId).ToList();
                var messages = await _unitOfWork.Repository<TbGroupsMessages>()
                    .FindAllAsync(m => groupIds.Contains(m.GroupId));

                var lastMessages = messages
                    .GroupBy(m => m.GroupId)
                    .Select(g => g.OrderByDescending(m => m.SentAt).FirstOrDefault())
                    .ToList();

                var groupsWithLastMessages = userGroups
                    .Select(g => new GroupWithLastMessage
                    {
                        GroupId = g.GroupId,
                        GroupName = g.Group.Name,
                        LastMessageContent = lastMessages.FirstOrDefault(m => m.GroupId == g.GroupId)?.MessageContent,
                        LastMessageSentAt = lastMessages.FirstOrDefault(m => m.GroupId == g.GroupId)?.SentAt
                    })
                    .OrderByDescending(g => g.LastMessageSentAt)
                    .ToList();

                result.IsSuccess = true;
                result.Context = groupsWithLastMessages;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetGroupsWithLastMessages));
                result.IsSuccess = false;
                result.Message = "An error occurred while retrieving groups with last messages.";
            }

            return result;
        }



    }
}
