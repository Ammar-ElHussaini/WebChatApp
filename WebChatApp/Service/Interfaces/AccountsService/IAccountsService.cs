using ApiOpWebE_C.Models;
using ApiOpWebE_C.OperationResults;

namespace WebChatApp.Service.Interfaces.AccountsService
{
    public interface IAccountsService
    {
        Task<CreationResult<ApplicationUser>> RegisterUser(UserLoginModel userDto);
        Task<CreationResult<ApplicationUser>> LoginUser(UserLoginModel userDto);
    }
}
