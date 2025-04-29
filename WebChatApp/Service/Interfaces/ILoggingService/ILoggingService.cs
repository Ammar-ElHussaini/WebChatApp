namespace WebChatApp.Service.Interfaces.iLoggingService
{
    public interface ILoggingService
    {
        Task LogErrorAsync(string message, string stackTrace, string methodName);
    }

}
