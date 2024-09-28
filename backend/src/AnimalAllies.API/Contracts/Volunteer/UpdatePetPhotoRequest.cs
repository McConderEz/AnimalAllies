using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.Commands.UpdatePetPhoto;

namespace AnimalAllies.API.Contracts.Volunteer;

public record UpdatePetPhotoRequest(IFormFileCollection Files)
{
    public UpdatePetPhotosCommand ToCommand(Guid volunteerId,Guid petId, IEnumerable<CreateFileDto> Photos)
        => new(volunteerId, petId, Photos);
}