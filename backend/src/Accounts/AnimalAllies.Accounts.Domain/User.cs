using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Domain;

public class User:IdentityUser<Guid>
{
    public string Photo { get; set; }
    public List<SocialNetwork> SocialNetworks { get; set; } = [];
}

