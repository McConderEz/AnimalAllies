using AnimalAllies.Core.DTOs.ValueObjects;

namespace AnimalAllies.Accounts.Contracts.Requests;

public record AddSocialNetworksRequest(IEnumerable<SocialNetworkDto> SocialNetworkDtos);