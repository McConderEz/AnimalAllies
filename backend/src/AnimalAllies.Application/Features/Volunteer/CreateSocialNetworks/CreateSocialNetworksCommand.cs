using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;

public record CreateSocialNetworksCommand(Guid Id,IEnumerable<SocialNetworkDto> SocialNetworks);
