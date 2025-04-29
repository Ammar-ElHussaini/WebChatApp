using ApiOpWebE_C.OperationResults;
using WebChatApp.Data_Acess_Layer.Models.DB;
using WebChatApp.Models;

namespace WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper
{
    public interface IGroupsUserManagement
    {
        Task<CreationResult<TbGroups>> UpdateGroupName(int groupId, string newGroupName);
        Task<CreationResult<TbGroups>> CreateGroup(string groupName, string userId);
        Task<CreationResult<bool>> DeleteGroup(int groupId);
    }
}
