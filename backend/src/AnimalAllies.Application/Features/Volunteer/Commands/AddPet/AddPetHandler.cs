using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Species.Breed;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Commands.AddPet;

public class AddPetHandler : ICommandHandler<AddPetCommand, Guid>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<AddPetHandler> logger,
        IDateTimeProvider dateTimeProvider,
        IValidator<AddPetCommand> validator,
        ISpeciesRepository speciesRepository)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
        _speciesRepository = speciesRepository;
    }
    

    public async Task<Result<Guid>> Handle(
        AddPetCommand command,
        CancellationToken cancellationToken = default)
    {

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }
        
        var volunteerResult = await _volunteerRepository.GetById(
            VolunteerId.Create(command.VolunteerId), cancellationToken);

        if (volunteerResult.IsFailure)
            return volunteerResult.Errors;

        var petId = PetId.NewGuid();
        
        var name = Name.Create(command.Name).Value;
        
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var helpStatus = HelpStatus.Create(command.HelpStatus).Value;
        
        var petPhysicCharacteristics = PetPhysicCharacteristics.Create(
            command.PetPhysicCharacteristics.Color,
            command.PetPhysicCharacteristics.HealthInformation,
            command.PetPhysicCharacteristics.Weight,
            command.PetPhysicCharacteristics.Height,
            command.PetPhysicCharacteristics.IsCastrated,
            command.PetPhysicCharacteristics.IsVaccinated).Value;

        var petDetails = PetDetails.Create(
            command.PetDetails.Description,
            DateOnly.FromDateTime(command.PetDetails.BirthDate),
            _dateTimeProvider.UtcNow).Value;

        var address = Address.Create(
            command.Address.Street,
            command.Address.City,
            command.Address.State,
            command.Address.ZipCode).Value;

        var isSpeciesExist = await _speciesRepository
            .GetById(SpeciesId.Create(command.AnimalType.SpeciesId), cancellationToken);
        if (isSpeciesExist.IsFailure)
            return isSpeciesExist.Errors;

        var isBreedExist = isSpeciesExist.Value
            .GetById(BreedId.Create(command.AnimalType.BreedId));
        if (isBreedExist.IsFailure)
            return isBreedExist.Errors;
        
        var animalType = new AnimalType(SpeciesId.Create(command.AnimalType.SpeciesId), command.AnimalType.BreedId);

        var requisites =
            new ValueObjectList<Requisite>(command.Requisites
                .Select(r => Requisite.Create(r.Title, r.Description).Value).ToList());
        
        
        var pet = new Pet(
            petId,
            name,
            petPhysicCharacteristics,
            petDetails,
            address,
            phoneNumber,
            helpStatus,
            animalType,
            requisites,
            new ValueObjectList<PetPhoto>([]));

        volunteerResult.Value.AddPet(pet);

        await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);
        
        _logger.LogInformation("added pet with id {petId} to volunteer with id {volunteerId}", petId.Id, volunteerResult.Value.Id.Id);
        
        return pet.Id.Id;
    }
}