using System.Text.RegularExpressions;

namespace WebChatApp.Data_Acess_Layer.Models.DB
{
    public class TbGroupsMessages
    {
        public int MessageId { get; set; }
        public int GroupId { get; set; }
        public string FromUser { get; set; }
        public string MessageContent { get; set; }
        public string GroupName { get; set; }

        public string EncryptedMessage { get; set; }  // الرسالة المشفرة
        public string EncryptedKey { get; set; }      // المفتاح المشفر باستخدام RSA
        public string Iv { get; set; }
        public string DecryptedMessage { get; set; }

        public DateTime SentAt { get; set; }

        public TbGroups Group { get; set; }
    }

}
