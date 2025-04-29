using System.Text.RegularExpressions;

namespace WebChatApp.Data_Acess_Layer.Models.DB
{
    public class TbGroupMember
    {
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime JoinedAt { get; set; }
        public Group Group { get; set; }
    }
}
