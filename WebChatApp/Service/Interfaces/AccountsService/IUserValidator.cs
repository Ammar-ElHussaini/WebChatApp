namespace WebChatApp.Service.Interfaces.AccountsService
{
    public interface IUserValidator
    {
        Task<ApplicationUser> GetUserByUsernameOrEmail(string username, string email);
    }
}
