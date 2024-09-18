using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.CreateSocialNetworks;

namespace AnimalAllies.API.Contracts.Volunteer;

public record CreateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public CreateSocialNetworksCommand ToCommand(Guid volunteerId)
        => new(volunteerId, SocialNetworks);
}