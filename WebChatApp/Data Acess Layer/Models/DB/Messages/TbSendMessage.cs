using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;

namespace WebChatApp.Models;

public partial class TbSendMessage
{
    public string IdPr { get; set; } = null!;
    public string FromUserId { get; set; } = null!;
    public string ToUserId { get; set; } = null!;
    public DateTime ConnectedAt { get; set; }
    public string EncryptedMessage { get; set; }
    public bool WasRead { get; set; }






}
