using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string SecondName { get; init;} = string.Empty;
    public string Patronymic { get; init;} = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int WorkExperience { get; set; }
    public string Requisites { get; init; } = string.Empty;
    public string SocialNetworks { get; init; } = string.Empty;
    //public RequisiteDto[] Requisites { get; set; } = [];
    //public SocialNetworkDto[] SocialNetworks { get; set; } = [];
}


