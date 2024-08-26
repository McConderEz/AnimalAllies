using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public record CreateSocialNetworksRequest(Guid Id,SocialNetworkListDto Dto);
