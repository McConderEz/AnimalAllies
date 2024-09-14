using AnimalAllies.Application.Extension;
using AnimalAllies.Application.FileProvider;
using AnimalAllies.Application.Providers;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;


namespace AnimalAllies.Application.Features.Volunteer.AddPet;

public class AddPetHandler
{
    private const string BUCKE_NAME = "photos";
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<AddPetHandler> logger,
        IDateTimeProvider dateTimeProvider,
        IValidator<AddPetCommand> validator)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
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