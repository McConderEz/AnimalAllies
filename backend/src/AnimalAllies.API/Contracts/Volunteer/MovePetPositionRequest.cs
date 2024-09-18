using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.MovePetPosition;

namespace AnimalAllies.API.Contracts.Volunteer;

public record MovePetPositionRequest(PositionDto Position)
{
    public MovePetPositionCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Position);
}