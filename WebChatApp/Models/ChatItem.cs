namespace WebChatApp.Models
{
    public class ChatItem
    {
        public string Id { get; set; } 
        public string Name { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public bool IsGroup { get; set; }
        public string ProfileImage { get; set; } 
    }

}
