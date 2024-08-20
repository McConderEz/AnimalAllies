using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer;

public record CreateVolunteerRequest(
    string FirstName,
    string SecondName,
    string? Patronymic,
    string Email,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites);
