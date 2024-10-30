using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace VolunteerRequests.Application.Features.Commands;

public record CreateVolunteerRequestCommand(
    Guid UserId,
    FullNameDto FullNameDto,
    string Email,
    string PhoneNumber,
    int WorkExperience,
    string VolunteerDescription,
    IEnumerable<SocialNetworkDto> SocialNetworkDtos) : ICommand;
