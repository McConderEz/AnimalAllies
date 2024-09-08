using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;

namespace AnimalAllies.API.Contracts;

public record CreateSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworks)
{
    public CreateSocialNetworksCommand ToCommand(Guid volunteerId)
        => new(volunteerId, SocialNetworks);
}