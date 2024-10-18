using System.Dynamic;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Domain;

public class User:IdentityUser<Guid>
{
    private User()
    {
        
    }
    
    public string? Photo { get; set; }
    public List<SocialNetwork> SocialNetworks { get; set; } = [];
    private List<Role> _roles = [];
    public IReadOnlyList<Role> Roles => _roles;
    
    public static User CreateAdmin(string userName, string email, Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role]
        };
    }

    public static User CreateParticipant(
        string userName,
        string email,
        IEnumerable<SocialNetwork> socialNetworks,
        Role role)
    {
        return new User
        {
            UserName = userName,
            Email = email,
            _roles = [role],
            SocialNetworks = socialNetworks.ToList()
        };
    }
}

