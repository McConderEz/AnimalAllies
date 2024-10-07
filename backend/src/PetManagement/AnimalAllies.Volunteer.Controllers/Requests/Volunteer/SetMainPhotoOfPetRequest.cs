using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.SetMainPhotoOfPet;

namespace AnimalAllies.Volunteer.Controllers.Requests.Volunteer;

public record SetMainPhotoOfPetRequest(string Path)
{
    public SetMainPhotoOfPetCommand ToCommand(Guid volunteerId, Guid petId)
        => new(volunteerId, petId, Path);
}