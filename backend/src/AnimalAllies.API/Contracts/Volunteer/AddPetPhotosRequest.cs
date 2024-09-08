using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.AddPetPhoto;

namespace AnimalAllies.API.Contracts;

public record AddPetPhotosRequest(IFormFileCollection Files)
{
    public AddPetPhotosCommand ToCommand(Guid VolunteerId,Guid PetId, IEnumerable<CreateFileDto> Photos)
        => new(VolunteerId, PetId, Photos);
}