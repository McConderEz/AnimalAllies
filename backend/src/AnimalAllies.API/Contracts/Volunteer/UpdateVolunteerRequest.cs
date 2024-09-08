using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;

namespace AnimalAllies.API.Contracts;

public record UpdateVolunteerRequest(UpdateVolunteerMainInfoDto Dto)
{
    public UpdateVolunteerCommand ToCommand(Guid volunteerId)
        => new(volunteerId, Dto);
}