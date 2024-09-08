using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.CreateRequisites;
using AnimalAllies.Application.Features.Volunteer.CreateVolunteer;

namespace AnimalAllies.API.Contracts;

public record CreateVolunteerRequest(
    FullNameDto FullName,
    string Email,
    string Description,
    int WorkExperience,
    string PhoneNumber,
    IEnumerable<SocialNetworkDto> SocialNetworks,
    IEnumerable<RequisiteDto> Requisites)
{
    public CreateVolunteerCommand ToCommand()
        => new(
            FullName,
            Email,
            Description,
            WorkExperience,
            PhoneNumber,
            SocialNetworks,
            Requisites);
}