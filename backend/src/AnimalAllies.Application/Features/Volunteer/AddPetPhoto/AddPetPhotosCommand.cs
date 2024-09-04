using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;

namespace AnimalAllies.Application.Features.Volunteer.AddPetPhoto;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileDto> Photos);
