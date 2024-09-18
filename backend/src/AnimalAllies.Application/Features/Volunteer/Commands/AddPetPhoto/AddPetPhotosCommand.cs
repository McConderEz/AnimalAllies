using AnimalAllies.Application.Contracts.DTOs;

namespace AnimalAllies.Application.Features.Volunteer.Commands.AddPetPhoto;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileDto> Photos);
