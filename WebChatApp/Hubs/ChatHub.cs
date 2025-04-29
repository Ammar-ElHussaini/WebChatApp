using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebChatApp.Models;

public class ChatHub : Hub
{
    public static Dictionary<string, string> UserConnections = new();

    private readonly IUnitOfWork _unitOfWork;
    private readonly WebchatAppContext _ss;


    public ChatHub(IUnitOfWork unitOfWork , WebchatAppContext ss)
    {
        _unitOfWork = unitOfWork;
        _ss = ss;
    }

    public override async Task OnConnectedAsync()
    {
        var username = Context.GetHttpContext().Request.Query["username"];
        if (!string.IsNullOrEmpty(username))
        {

            var userOnline = new TbUserOnline
            {
                IdPr = Guid.NewGuid().ToString(),
                UserName = username,
                Idconctoin = Context.ConnectionId,
                ConnectedAt = DateTime.UtcNow
            };

              await _unitOfWork.Repository<TbUserOnline>().AddAsync(userOnline);
            await _unitOfWork.CompleteAsync(); 
        }

        // استدعاء الـ base method
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;

        var userOnline = _ss.UserOnlines.FirstOrDefault(s => s.Idconctoin == connectionId) ;
        if (userOnline != null)
        {
             _unitOfWork.Repository<TbUserOnline>().Delete(userOnline);
            await _unitOfWork.CompleteAsync(); 
        }

        // استدعاء الـ base method
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendPrivateMessage(string toUsername, string message)
    {
        var from = await _unitOfWork.Repository<TbUserOnline>()
       .GetAllAsync()
       .ContinueWith(task => task.Result.FirstOrDefault(x => x.Idconctoin == Context.ConnectionId));

        if (from != null)
        {
            await Clients.Client(from.Idconctoin).SendAsync("ReceiveMessage", from, message, true, DateTime.Now);
        }

        await  _unitOfWork.Repository<TbUserOnline>().AddAsync(from);
        await _unitOfWork.CompleteAsync();

    }

    public async Task ReceiveMessageAsync(string message)
    {
        var from = await _unitOfWork.Repository<TbUserOnline>()
            .GetAllAsync()
            .ContinueWith(task => task.Result.FirstOrDefault(x => x.Idconctoin == Context.ConnectionId));

        if (from != null)
        {
            await Clients.Client(from.Idconctoin).SendAsync("ReceiveMessage", from, message, false, DateTime.Now);

        }
    }


}
