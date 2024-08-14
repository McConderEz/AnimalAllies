using AnimalAllies.Domain.DTOs;
using AnimalAllies.Domain.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs.Volunteer;

public record class CreateVolunteerRequest(
    string FirstName,
    string SecondName,
    string Patronymic,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    List<SocialNetworkDto> SocialNetworks,
    List<RequisiteDto> Requisites);
