using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeletePetPhoto;

public record DeletePetPhotosCommand(Guid VolunteerId, Guid PetId) : ICommand;
