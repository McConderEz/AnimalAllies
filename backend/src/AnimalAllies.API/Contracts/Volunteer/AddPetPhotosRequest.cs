using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPetPhoto;

namespace AnimalAllies.API.Contracts.Volunteer;

public record AddPetPhotosRequest(IFormFileCollection Files)
{
    public AddPetPhotosCommand ToCommand(Guid volunteerId,Guid petId, IEnumerable<CreateFileDto> photos)
        => new(volunteerId, petId, photos);
}