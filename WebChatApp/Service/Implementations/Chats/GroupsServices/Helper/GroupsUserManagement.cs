using ApiOpWebE_C.OperationResults;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebChatApp.Data_Acess_Layer.Models.DB;
using WebChatApp.Models;
using WebChatApp.Service.Interfaces.IChats.IGroupsServices.Helper;
using WebChatApp.Service.Interfaces.iLoggingService;

namespace WebChatApp.Service.Implementations.Chats.GroupsServices.Helper
{
    public class GroupsUserManagement : IGroupsUserManagement
    {
        private readonly IUnitOfWork _unitOfWork;
        ILoggingService _loggingService;
        public GroupsUserManagement(IUnitOfWork unitOfWork, DbContext context, ILoggingService loggingService)
        {
            _unitOfWork = unitOfWork;
            _loggingService = loggingService;
        }

        public async Task<CreationResult<TbGroups>> UpdateGroupName(int groupId, string newGroupName)
        {
            var result = new CreationResult<TbGroups>();
            try
            {
                // البحث عن المجموعة باستخدام groupId
                var group = await _unitOfWork.Repository<TbGroups>().FindAsync(g => g.GroupId == groupId);

                if (group != null)
                {
                    // تحديث اسم المجموعة
                    group.GroupName = newGroupName;

                    // حفظ التغييرات في قاعدة البيانات
                    await _unitOfWork.CompleteAsync();

                    result.IsSuccess = true;
                    result.Message = "Group name updated successfully.";
                    result.Context = group;  // إرجاع الكائن المحدث (المجموعة)
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Group not found.";
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(UpdateGroupName));

                result.IsSuccess = false;
                result.Errors.Add(ex.Message);
                result.Message = "An error occurred while updating the group name.";
            }
            return result;
        }

        public async Task<CreationResult<TbGroups>> CreateGroup(string groupName, string userId)
        {
            var result = new CreationResult<TbGroups>();
            try
            {
                // التحقق من صحة المدخلات
                if (string.IsNullOrEmpty(groupName))
                {
                    result.IsSuccess = false;
                    result.Message = "Group name cannot be empty.";
                    return result;
                }

                // إنشاء الكائن الجديد للمجموعة
                var group = new TbGroups
                {
                    GroupName = groupName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId  // تحديد من قام بإنشاء المجموعة
                };

                var GroupMember = new TbGroupMember
                {
                    UserId = userId,
                    IsAdmin = true  // تحديد من قام بإنشاء المجموعة
                };

                // إضافة المجموعة إلى قاعدة البيانات
                await _unitOfWork.Repository<TbGroups>().AddAsync(group);
                await _unitOfWork.Repository<TbGroupMember>().AddAsync(GroupMember);
                await _unitOfWork.CompleteAsync();

                result.IsSuccess = true;
                result.Message = "Group created successfully.";
                result.Context = group;  // إرجاع المجموعة التي تم إنشاؤها
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(CreateGroup));

                result.IsSuccess = false;
                result.Errors.Add(ex.Message);
                result.Message = "An error occurred while creating the group.";
            }
            return result;
        }
        public async Task<CreationResult<bool>> DeleteGroup(int groupId)
        {
            var result = new CreationResult<bool>();
            try
            {
                var group = await _unitOfWork.Repository<TbGroups>().FindAsync(g => g.GroupId == groupId);
                var groupMember = await _unitOfWork.Repository<TbGroupMember>().FindAsync(g => g.GroupId == groupId);

                if (group != null)
                {
                    _unitOfWork.Repository<TbGroups>().Delete(group);
                    _unitOfWork.Repository<TbGroupMember>().Delete(groupMember);
                    await _unitOfWork.CompleteAsync();

                    result.IsSuccess = true;
                    result.Message = "Group deleted successfully.";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Group not found.";
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(DeleteGroup));

                result.IsSuccess = false;
                result.Message = "An error occurred while deleting the group.";
            }
            return result;
        }




    }
}
