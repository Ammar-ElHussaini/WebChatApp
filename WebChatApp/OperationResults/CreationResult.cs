using Microsoft.AspNetCore.Identity;

namespace ApiOpWebE_C.OperationResults
{
    public class CreationResult<T>
    {
        public T Context { get; set; }
        public List<T> ContextList { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool IsSuccess { get; set; }
        public string Message { get; set; }


    }

}
