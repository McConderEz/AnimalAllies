using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.UpdatePetStatus;

namespace AnimalAllies.Volunteer.Controllers.Requests.Volunteer;

public record UpdatePetStatusRequest(string HelpStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, HelpStatus);
}