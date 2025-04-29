namespace WebChatApp.Service.Interfaces.IChats.IChatService.Helper
{
    public interface IUserStatusService
    {
        Task<bool> IsUserOnlineAsync(string username);
    }

}
