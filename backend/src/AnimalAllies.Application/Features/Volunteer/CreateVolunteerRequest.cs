using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites);
