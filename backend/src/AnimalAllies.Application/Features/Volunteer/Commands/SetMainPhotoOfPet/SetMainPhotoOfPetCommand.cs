
using AnimalAllies.Application.Abstractions;

namespace AnimalAllies.Application.Features.Volunteer.Commands.SetMainPhotoOfPet;

public record SetMainPhotoOfPetCommand(Guid VolunteerId, Guid PetId, string Path) : ICommand;
