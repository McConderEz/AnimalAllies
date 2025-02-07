using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.AddPetPhoto;
using Microsoft.AspNetCore.Http;

namespace AnimalAllies.Volunteer.Presentation.Requests.Volunteer;

public record AddPetPhotosRequest(IFormFileCollection Files)
{
    public AddPetPhotosCommand ToCommand(Guid volunteerId,Guid petId, IEnumerable<CreateFileDto> photos)
        => new(volunteerId, petId, photos);
}