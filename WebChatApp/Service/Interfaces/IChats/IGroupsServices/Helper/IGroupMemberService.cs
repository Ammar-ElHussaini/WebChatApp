using ApiOpWebE_C.OperationResults;
using WebChatApp.Data_Acess_Layer.Models.DB;

namespace WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper
{
    public interface IGroupMemberService
    {
            Task<CreationResult<TbGroupMember>> AddUserToGroup(int groupId, string userId);
            Task<CreationResult<TbGroupMember>> RemoveUserFromGroup(int groupId, string userId);
            Task<CreationResult<List<TbGroupMember>>> GetMembersOfGroup(int groupId);
            Task<CreationResult<List<TbGroups>>> GetUserGroups(string userId);
            Task<CreationResult<int>> GetGroupMembersCount(int groupId);
            Task<CreationResult<TbGroupMember>> LeaveGroup(int groupId, string userId);
            Task<CreationResult<TbGroupMember>> JoinGroup(int groupId, string userId);
            Task<CreationResult<TbGroupMember>> PromoteUserToAdmin(int groupId, string userId);
            Task<CreationResult<TbGroupMember>> DemoteAdminToUser(int groupId, string userId);
            Task<CreationResult<List<TbGroupMember>>> GetGroupAdmins(int groupId);



    }
}
