namespace WebChatApp.Data_Acess_Layer.Models.DB
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string MethodName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
