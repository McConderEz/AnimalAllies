using AnimalAllies.Application.Features.Volunteer.Commands.SetMainPhotoOfPet;

namespace AnimalAllies.API.Contracts.Volunteer;

public record SetMainPhotoOfPetRequest(string Path)
{
    public SetMainPhotoOfPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Path);
}