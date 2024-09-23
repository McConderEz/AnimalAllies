using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.Application.Features.Volunteer.Commands.MovePetPosition;

public record MovePetPositionCommand(Guid VolunteerId, Guid PetId, PositionDto Position) : ICommand;
