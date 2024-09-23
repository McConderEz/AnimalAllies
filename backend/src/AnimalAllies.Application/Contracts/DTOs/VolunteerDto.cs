using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Contracts.DTOs;

public class VolunteerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set;} = string.Empty;
    public string Patronymic { get; set;} = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int WorkExperience { get; set; }
    public RequisiteDto[] Requisites { get; set; } = [];
    public SocialNetworkDto[] SocialNetworks { get; set; } = [];
}


