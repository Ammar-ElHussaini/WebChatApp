using ApiOpWebE_C.OperationResults;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebChatApp.Data_Acess_Layer.Models.DB;
using WebChatApp.Models;

namespace WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper
{
    public interface IGroupsMessagesService
    {
        Task<CreationResult<TbGroupsMessages>> AddMessageToGroup(int groupId, string messageContent, string userId);

        Task<CreationResult<TbGroupsMessages>> EditMessageInGroup(int groupId, int messageId, string newContent, string userId);

        Task<CreationResult<bool>> DeleteMessageFromGroup(int groupId, int messageId, string userId);

        Task<CreationResult<List<TbGroupsMessages>>> GetMessagesFromGroupAsync(int groupId, string userId);

        Task<CreationResult<List<TbGroupsMessages>>> GetUnreadMessagesForUser(int groupId, string userId);

        Task<CreationResult<List<GroupWithLastMessage>>> GetGroupsWithLastMessages(string userId);
    }
}
