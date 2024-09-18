using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.Commands.MovePetPosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, PositionDto Position);
