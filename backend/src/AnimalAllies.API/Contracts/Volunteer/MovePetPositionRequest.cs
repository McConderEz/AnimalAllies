using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.MovePetPosition;
using AnimalAllies.Domain.Models.Volunteer.Pet;

namespace AnimalAllies.API.Contracts;

public record MovePetPositionRequest(PositionDto Position)
{
    public MovePetPositionCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Position);
}