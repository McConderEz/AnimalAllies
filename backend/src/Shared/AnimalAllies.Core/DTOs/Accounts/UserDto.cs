using AnimalAllies.Core.DTOs.ValueObjects;

namespace AnimalAllies.Core.DTOs.Accounts;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string? Photo { get; set; }
    public SocialNetworkDto[] SocialNetworks { get; set; } = [];
    public RoleDto[] Roles { get; set; } = [];
    public VolunteerAccountDto? VolunteerAccount { get; set; }
    public ParticipantAccountDto? ParticipantAccountDto { get; set; }
}
