using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.DTOs.ValueObjects;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CreateSocialNetworks;

public record CreateSocialNetworksCommand(Guid Id,IEnumerable<SocialNetworkDto> SocialNetworks) : ICommand;
