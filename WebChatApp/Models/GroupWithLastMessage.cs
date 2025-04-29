namespace WebChatApp.Models
{
    public class GroupWithLastMessage
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime? LastMessageSentAt { get; set; }
    }
}
