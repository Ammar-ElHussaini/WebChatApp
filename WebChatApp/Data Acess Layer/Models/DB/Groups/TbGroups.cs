namespace WebChatApp.Data_Acess_Layer.Models.DB
{
    public class TbGroups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }

        public ICollection<TbGroupMember> Members { get; set; }
        public ICollection<TbGroupsMessages> Messages { get; set; }
    }
}
