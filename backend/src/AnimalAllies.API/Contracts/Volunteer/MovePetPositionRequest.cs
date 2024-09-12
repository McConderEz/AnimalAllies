using AnimalAllies.Application.Features.Volunteer.MovePetPosition;
using AnimalAllies.Domain.Models.Volunteer.Pet;

namespace AnimalAllies.API.Contracts;

public record MovePetPositionRequest(Position Position)
{
    public MovePetPositionCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Position);
}