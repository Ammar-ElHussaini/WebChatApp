using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using WebChatApp.Models;
using WebChatApp.Service.Interfaces.IChats.IChatService.Helper;

public class UserStatusService : IUserStatusService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserStatusService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> IsUserOnlineAsync(string username)
    {
        var userStatus = await _unitOfWork.Repository<TbUserOnline>()
            .GetAllAsync(u => u.UserName == username);

        return userStatus != null && userStatus.Any();
    }
}
