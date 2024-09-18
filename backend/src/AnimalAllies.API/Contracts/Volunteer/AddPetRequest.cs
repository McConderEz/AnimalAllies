using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

namespace AnimalAllies.API.Contracts.Volunteer;

public record AddPetRequest(
    string Name,
    PetPhysicCharacteristicsDto PetPhysicCharacteristicsDto,
    PetDetailsDto PetDetailsDto,
    AddressDto AddressDto,
    string PhoneNumber,
    string HelpStatus,
    AnimalTypeDto AnimalTypeDto,
    IEnumerable<RequisiteDto> RequisitesDto)
{
    public AddPetCommand ToCommand(Guid volunteerId)
        => new(volunteerId,
            Name,
            PetPhysicCharacteristicsDto,
            PetDetailsDto,
            AddressDto,
            PhoneNumber,
            HelpStatus,
            AnimalTypeDto,
            RequisitesDto
        );
}
    
    