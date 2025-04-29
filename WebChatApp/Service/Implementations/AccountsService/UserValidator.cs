using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebChatApp.Service.Interfaces.AccountsService;

namespace WebChatApp.Service.Implementations.AccountsService
{
    public class UserValidator : IUserValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ;
        }

        public async Task<ApplicationUser> GetUserByUsernameOrEmail(string username, string email)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == username || u.Email == email);
        }
    }
}
