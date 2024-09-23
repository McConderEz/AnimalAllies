using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.Application.Features.Volunteer.Commands.CreateSocialNetworks;

public record CreateSocialNetworksCommand(Guid Id,IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
