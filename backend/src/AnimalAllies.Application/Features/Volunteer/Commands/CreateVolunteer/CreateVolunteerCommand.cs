using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.Commands.CreateVolunteer;

public record CreateVolunteerCommand(
    FullNameDto FullName,
    string Email,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites);
