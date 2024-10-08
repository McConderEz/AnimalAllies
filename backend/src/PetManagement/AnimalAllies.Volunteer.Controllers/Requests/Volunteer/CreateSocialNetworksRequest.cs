using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CreateSocialNetworks;

namespace AnimalAllies.Volunteer.Presentation.Requests.Volunteer;

public record CreateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public CreateSocialNetworksCommand ToCommand(Guid volunteerId)
        => new(volunteerId, SocialNetworks);
}