using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;

namespace AnimalAllies.Application.Features.Volunteer.MovePetPosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, PositionDto Position);
