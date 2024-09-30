
using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeletePetSoft;

public record DeletePetSoftCommand(Guid VolunteerId, Guid PetId) : ICommand;
