using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.Commands.CreateSocialNetworks;

public record CreateSocialNetworksCommand(Guid Id,IEnumerable<SocialNetworkDto> SocialNetworks);
