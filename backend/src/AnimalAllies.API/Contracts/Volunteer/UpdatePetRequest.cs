using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Features.Volunteer.Commands.UpdatePet;

namespace AnimalAllies.API.Contracts.Volunteer;

public record UpdatePetRequest(
    string Name,
    PetPhysicCharacteristicsDto PetPhysicCharacteristicsDto,
    PetDetailsDto PetDetailsDto,
    AddressDto AddressDto,
    string PhoneNumber,
    string HelpStatus,
    AnimalTypeDto AnimalTypeDto,
    IEnumerable<RequisiteDto> RequisitesDto)
{
    public UpdatePetCommand ToCommand(Guid volunteerId,Guid petId)
        => new(volunteerId,
            petId,
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