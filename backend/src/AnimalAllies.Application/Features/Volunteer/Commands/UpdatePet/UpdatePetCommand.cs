
using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdatePet;

public record UpdatePetCommand(
    Guid VolunteerId,
    Guid PetId,
    string Name,
    PetPhysicCharacteristicsDto PetPhysicCharacteristicsDto,
    PetDetailsDto PetDetailsDto,
    AddressDto AddressDto,
    string PhoneNumber,
    string HelpStatus,
    AnimalTypeDto AnimalTypeDto,
    IEnumerable<RequisiteDto> RequisiteDtos) : ICommand;
