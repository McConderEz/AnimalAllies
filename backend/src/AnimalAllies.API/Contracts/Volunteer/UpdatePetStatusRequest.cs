using AnimalAllies.Application.Features.Volunteer.Commands.UpdatePetStatus;

namespace AnimalAllies.API.Contracts.Volunteer;

public record UpdatePetStatusRequest(string HelpStatus)
{
    public UpdatePetStatusCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, HelpStatus);
}