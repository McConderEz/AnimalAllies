using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeletePetForce;

public record DeletePetForceCommand(Guid VolunteerId, Guid PetId) : ICommand;