using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.Commands.UpdateVolunteer;

namespace AnimalAllies.API.Contracts.Volunteer;

public record UpdateVolunteerRequest(UpdateVolunteerMainInfoDto Dto)
{
    public UpdateVolunteerCommand ToCommand(Guid volunteerId)
        => new(volunteerId, Dto);
}