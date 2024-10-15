using System.Dynamic;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Domain;

public class User:IdentityUser<Guid>
{
    public string? Photo { get; set; }
    public List<SocialNetwork>? SocialNetworks { get; set; } = [];
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    
    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            Role = role
        };
    }
}

