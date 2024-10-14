using System.Dynamic;
using Microsoft.AspNetCore.Identity;

namespace AnimalAllies.Accounts.Domain;

public class User:IdentityUser<Guid>
{
    public string? Photo { get; set; }
    public List<SocialNetwork>? SocialNetworks { get; set; } = [];
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    public List<ParticipantAccount> ParticipantAccounts { get; set; } = [];
    public List<VolunteerAccount> VolunteerAccounts { get; set; } = [];
    public List<AdminProfile> AdminProfiles { get; set; } = [];
}

