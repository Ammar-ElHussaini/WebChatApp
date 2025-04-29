using System;
using System.Collections.Generic;

namespace WebChatApp.Models;

public partial class TbUserOnline
{
    public string IdPr { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Idconctoin { get; set; } = null!;

    public DateTime ConnectedAt { get; set; }

}
