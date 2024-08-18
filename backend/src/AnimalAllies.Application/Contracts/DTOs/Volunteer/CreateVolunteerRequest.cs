using AnimalAllies.Domain.DTOs;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs.Volunteer;

public record CreateVolunteerRequest(
    string FirstName,
    string SecondName,
    string Patronymic,
    string Email,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites);
