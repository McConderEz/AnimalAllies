using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.Application.Features.Volunteer.Commands.AddPetPhoto;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<CreateFileDto> Photos) : ICommand;
