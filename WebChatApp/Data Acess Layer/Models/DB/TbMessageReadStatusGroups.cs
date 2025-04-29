namespace WebChatApp.Models
{
    public class TbMessageReadStatusGroups
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public int MessageId { get; set; }
        public DateTime? ReadAt { get; set; } 
    }
}
