using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string SecondName { get; init;} = string.Empty;
    public string Patronymic { get; init;} = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public int WorkExperience { get; init; }
    public RequisiteDto[] Requisites { get; set; } = [];
    public SocialNetworkDto[] SocialNetworks { get; set; } = [];
}


