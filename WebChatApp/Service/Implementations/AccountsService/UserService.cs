using ApiOpWebE_C.OperationResults;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using WebChatApp.DTO;
using WebChatApp.Service.Interfaces.AccountsService;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;
using WebChatApp.Service.Interfaces.iLoggingService;

namespace WebChatApp.Service.Implementations.AccountsService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageService _messageService;
        private readonly ILoggingService _loggingService;


        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMessageService messageService, ILoggingService loggingService
)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _messageService = messageService;
            _loggingService = loggingService;
        }

        public async Task<CreationResult<UserChatInfo>> UpdateUserProfileAsync(string userId, string newUsername, string displayName, string bio)
        {
            var result = new CreationResult<UserChatInfo>();

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = "User not found.";
                    return result;
                }

                if (!string.IsNullOrEmpty(newUsername))
                    user.UserName = newUsername;

                if (!string.IsNullOrEmpty(displayName))
                    user.DisplayName = displayName;

                if (!string.IsNullOrEmpty(bio))
                    user.Bio = bio;

                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    result.IsSuccess = false;
                    result.Message = "Failed to update user profile.";
                    return result;
                }

                result.IsSuccess = true;
                result.Message = "User profile updated successfully.";
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync(ex.Message, ex.StackTrace, nameof(UpdateUserProfileAsync));

                result.IsSuccess = false;
                result.Message = "An error occurred while updating the profile.";
            }

            return result;
        }

        public async Task <CreationResult<UserChatInfo>> DeleteUserAccountAsync(string userId)
        {
            var result = new CreationResult<UserChatInfo>();

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = "User not found.";
                    return result;
                }

                var deletionResult = await _userManager.DeleteAsync(user);
                if (deletionResult.Succeeded)
                {
                    result.IsSuccess = true;
                    result.Message = "User account deleted successfully.";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Failed to delete user account.";
                    result.Errors = deletionResult.Errors.Select(e => e.Description).ToList();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "An error occurred while deleting the user account.";
                result.Errors.Add(ex.Message);
            }

            return result;
        }



        public async Task<CreationResult<UserChatInfo>> UpdateProfileImageAsync(string userId, string newProfileImageUrl)
        {
            var result = new CreationResult<UserChatInfo>();

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = "User not found.";
                    return result;
                }

                user.ProfileImage = newProfileImageUrl;

                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    result.IsSuccess = true;
                    result.Message = "Profile image updated successfully.";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Failed to update profile image.";
                    result.Errors = updateResult.Errors.Select(e => e.Description).ToList();
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "An error occurred while updating the profile image.";
                result.Errors.Add(ex.Message);
            }

            return result;
        }

    }
}
