namespace WebChatApp.DTO
{
    public class UserChatInfo
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImage { get; set; }


        public int NumberMessage { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
        public string LastMessageContent { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
