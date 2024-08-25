using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer;

public record CreateSocialNetworksRequest(Guid Id,IEnumerable<SocialNetworkDto> SocialNetworks);
