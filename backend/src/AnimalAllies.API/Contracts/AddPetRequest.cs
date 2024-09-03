using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AnimalAllies.API.Contracts;

public record AddPetRequest(
    string Name,
    PetPhysicCharacteristicsDto PetPhysicCharacteristicsDto,
    PetDetailsDto PetDetailsDto,
    AddressDto AddressDto,
    string PhoneNumber,
    string HelpStatus,
    AnimalTypeDto AnimalTypeDto,
    IEnumerable<RequisiteDto> RequisitesDto,
    IFormFileCollection Files);