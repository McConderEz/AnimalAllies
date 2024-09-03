using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;

namespace AnimalAllies.Application.Features.Volunteer.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    PetPhysicCharacteristicsDto PetPhysicCharacteristics,
    PetDetailsDto PetDetails,
    AddressDto Address,
    string PhoneNumber,
    string HelpStatus,
    AnimalTypeDto AnimalType,
    IEnumerable<RequisiteDto> Requisites,
    IEnumerable<CreateFileDto> Photos);
