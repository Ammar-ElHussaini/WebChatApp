using ApiOpWebE_C.OperationResults;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebChatApp.Data_Acess_Layer.Models.DB;
using WebChatApp.Models;
using WebChatApp.Service.Implementations.LoggingService;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Service.Interfaces.iLoggingService;

namespace WebChatApp.Service.Implementations.Chats.GroupsServices.Helper
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoggingService _loggingService;

        public GroupMemberService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _loggingService = loggingService;
        }

        public async Task<CreationResult<TbGroupMember>> AddUserToGroup(int groupId, string userId)
        {
            var result = new CreationResult<TbGroupMember>();
            try
            {
                var AddUserContext = new TbGroupMember
                {
                    GroupId = groupId,
                    UserId = userId,
                    JoinedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<TbGroupMember>().AddAsync(AddUserContext);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Context = AddUserContext;
                result.Message = GroupMemberErrorMessages.UserAddedToGroupSuccessfully;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(AddUserToGroup));
                result.IsSuccess = false;
                result.Errors.Add(ex.Message);
                result.Message = GroupMemberErrorMessages.AddUserToGroupError;
            }
            return result;
        }

        public async Task<CreationResult<TbGroupMember>> RemoveUserFromGroup(int groupId, string userId)
        {
            var result = new CreationResult<TbGroupMember>();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.UserNotFoundInGroupError;
                    return result;
                }

                var member = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == user.Id);

                if (member != null)
                {
                    _unitOfWork.Repository<TbGroupMember>().Delete(member);
                    await _unitOfWork.CompleteAsync();

                    result.IsSuccess = true;
                    result.Message = GroupMemberErrorMessages.UserRemovedFromGroupSuccessfully;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.UserNotFoundInGroupError;
                    return result;

                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(RemoveUserFromGroup));
                result.IsSuccess = false;
                result.Errors.Add(ex.Message);
                result.Message = GroupMemberErrorMessages.RemoveUserFromGroupError;
            }
            return result;
        }

        public async Task<CreationResult<List<TbGroupMember>>> GetMembersOfGroup(int groupId)
        {
            var result = new CreationResult<List<TbGroupMember>>();
            try
            {
                var members = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAllAsync(gm => gm.GroupId == groupId);

                if (members != null)
                {

                    result.IsSuccess = true;
                    result.Context = members;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.UserNotFoundInGroupError;

                }

            }

            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetMembersOfGroup));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.GetMembersOfGroupError;
            }
            return result;
        }

        public async Task<CreationResult<List<TbGroups>>> GetUserGroups(string userId)
        {
            var result = new CreationResult<List<TbGroups>>();
            try
            {
                var groups = await _unitOfWork.Repository<TbGroups>()
                    .FindAllAsync(g => g.Members.Any(m => m.UserId == userId));

                if (groups != null)
                {
                    result.IsSuccess = true;
                    result.Context = groups.OrderBy(g => g.CreatedAt).ToList();

                }
                else
                {
                    result.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetUserGroups));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.GetUserGroupsError;
            }
            return result;
        }

        public async Task<CreationResult<int>> GetGroupMembersCount(int groupId)
        {
            var result = new CreationResult<int>();
            try
            {
                var count = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAllAsync(gm => gm.GroupId == groupId);

                if (count != null)
                {
                    result.IsSuccess = true;
                    result.Context = count.Count;

                }
                else
                {
                    result.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetGroupMembersCount));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.GetMembersOfGroupError;
            }
            return result;
        }

        public async Task<CreationResult<TbGroupMember>> LeaveGroup(int groupId, string userId)
        {
            var result = new CreationResult<TbGroupMember>();
            try
            {
                var member = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

                if (member != null)
                {
                    _unitOfWork.Repository<TbGroupMember>().Delete(member);
                    await _unitOfWork.CompleteAsync();
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.UserNotFoundInGroupError;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(LeaveGroup));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.LeaveGroup;
            }
            return result;
        }

        public async Task<CreationResult<TbGroupMember>> JoinGroup(int groupId, string userId)
        {
            var result = new CreationResult<TbGroupMember>();
            try
            {
                var exists = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

                if (exists == null)
                {
                    var m = new TbGroupMember { GroupId = groupId, UserId = userId };
                    await _unitOfWork.Repository<TbGroupMember>().AddAsync(m);
                    await _unitOfWork.CompleteAsync();

                    result.IsSuccess = true;
                    result.Context = m;
                    result.Message = GroupMemberErrorMessages.UserAddedToGroupSuccessfully;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.ErrorUserAddedToGroupSuccessfully;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(JoinGroup));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.ErrorUserAddedToGroupSuccessfully;
            }
            return result;
        }

        public async Task<CreationResult<TbGroupMember>> PromoteUserToAdmin(int groupId, string userId)
        {
            var result = new CreationResult<TbGroupMember>();
            try
            {
                var member = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

                if (member != null)
                {
                    member.IsAdmin = true;
                    _unitOfWork.Repository<TbGroupMember>().Update(member);
                    await _unitOfWork.CompleteAsync();

                    result.IsSuccess = true;
                    result.Context = member;
                    result.Message = GroupMemberErrorMessages.DemoteAdminToUserError;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.UserNotFoundInGroupError;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(PromoteUserToAdmin));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.PromoteUserToAdminError;
            }
            return result;
        }

        public async Task<CreationResult<TbGroupMember>> DemoteAdminToUser(int groupId, string userId)
        {
            var result = new CreationResult<TbGroupMember>();
            try
            {
                var member = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAsync(gm => gm.GroupId == groupId && gm.UserId == userId);

                if (member != null)
                {
                    member.IsAdmin = false;
                    _unitOfWork.Repository<TbGroupMember>().Update(member);
                    await _unitOfWork.CompleteAsync();

                    result.IsSuccess = true;
                    result.Message = result.Message = GroupMemberErrorMessages.DemoteAdminToUserError;
                    ;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = GroupMemberErrorMessages.UserNotFoundInGroupError;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(DemoteAdminToUser));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.DemoteAdminToUserError;
            }
            return result;
        }

        public async Task<CreationResult<List<TbGroupMember>>> GetGroupAdmins(int groupId)
        {
            var result = new CreationResult<List<TbGroupMember>>();
            try
            {
                var admins = await _unitOfWork.Repository<TbGroupMember>()
                    .FindAllAsync(gm => gm.GroupId == groupId && gm.IsAdmin);

                if (admins.Any())
                {

                    result.IsSuccess = true;
                    return result;

                }
                else
                {
                    result.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(GetGroupAdmins));
                result.IsSuccess = false;
                result.Message = GroupMemberErrorMessages.GetGroupAdminsError;
            }
            return result;
        }
    }
}
