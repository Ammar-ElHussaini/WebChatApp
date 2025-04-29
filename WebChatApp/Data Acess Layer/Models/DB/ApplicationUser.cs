using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{

    public DateTime? LastSeen { get; set; }
    public string ProfileImage { get; set; }
    public string DisplayName { get; set; }
    public string Bio { get; set; }

}
