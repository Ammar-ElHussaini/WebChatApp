using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using WebChatApp.Data_Acess_Layer.Models.DB;
using WebChatApp.Service.Interfaces.iLoggingService;

namespace WebChatApp.Service.Implementations.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoggingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogErrorAsync(string message, string stackTrace, string methodName)
        {
            var errorLog = new ErrorLog
            {
                Message = message,
                StackTrace = stackTrace,
                MethodName = methodName,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<ErrorLog>().AddAsync(errorLog);
            await _unitOfWork.CompleteAsync();
        }
    }

}
