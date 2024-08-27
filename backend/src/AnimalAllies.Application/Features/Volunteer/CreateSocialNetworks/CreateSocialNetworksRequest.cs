using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;

public record CreateSocialNetworksRequest(Guid Id,SocialNetworkListDto Dto);
