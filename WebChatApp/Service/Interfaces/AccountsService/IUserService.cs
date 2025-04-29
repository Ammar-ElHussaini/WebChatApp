using ApiOpWebE_C.OperationResults;
using WebChatApp.DTO;

namespace WebChatApp.Service.Interfaces.AccountsService
{
    public interface IUserService
    {
        Task<CreationResult<UserChatInfo>> UpdateUserProfileAsync(string userId, string newUsername, string displayName, string bio);
        Task<CreationResult<UserChatInfo>> DeleteUserAccountAsync(string userId);
        Task<CreationResult<UserChatInfo>> UpdateProfileImageAsync(string userId, string newProfileImageUrl);
    }
}
