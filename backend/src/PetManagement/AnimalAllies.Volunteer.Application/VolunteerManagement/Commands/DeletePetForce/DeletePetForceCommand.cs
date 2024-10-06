using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeletePetForce;

public record DeletePetForceCommand(Guid VolunteerId, Guid PetId) : ICommand;