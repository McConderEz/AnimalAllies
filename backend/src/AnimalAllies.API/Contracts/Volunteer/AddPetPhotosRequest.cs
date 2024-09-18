using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPetPhoto;

namespace AnimalAllies.API.Contracts.Volunteer;

public record AddPetPhotosRequest(IFormFileCollection Files)
{
    public AddPetPhotosCommand ToCommand(Guid VolunteerId,Guid PetId, IEnumerable<CreateFileDto> Photos)
        => new(VolunteerId, PetId, Photos);
}