﻿using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Repositories;
using AnimalAllies.Domain.Common;
using AnimalAllies.Domain.Models.Species;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdatePet;

public class UpdatePetHandler: ICommandHandler<UpdatePetCommand, Guid>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IValidator<UpdatePetCommand> _validator;

    public UpdatePetHandler(
        IVolunteerRepository volunteerRepository,
        ILogger<UpdatePetHandler> logger,
        IDateTimeProvider dateTimeProvider,
        IValidator<UpdatePetCommand> validator,
        IReadDbContext readDbContext,
        IUnitOfWork unitOfWork)
    {
        _volunteerRepository = volunteerRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _validator = validator;
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
    }
    

    public async Task<Result<Guid>> Handle(
        UpdatePetCommand command,
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

        var petId = PetId.Create(command.PetId);

        var petIsExist = volunteerResult.Value.GetPetById(petId);
        if (petIsExist.IsFailure)
            return petIsExist.Errors;
        
        var name = Name.Create(command.Name).Value;
        
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        
        var helpStatus = HelpStatus.Create(command.HelpStatus).Value;
        
        var petPhysicCharacteristics = PetPhysicCharacteristics.Create(
            command.PetPhysicCharacteristicsDto.Color,
            command.PetPhysicCharacteristicsDto.HealthInformation,
            command.PetPhysicCharacteristicsDto.Weight,
            command.PetPhysicCharacteristicsDto.Height,
            command.PetPhysicCharacteristicsDto.IsCastrated,
            command.PetPhysicCharacteristicsDto.IsVaccinated).Value;

        var petDetails = PetDetails.Create(
            command.PetDetailsDto.Description,
            DateOnly.FromDateTime(command.PetDetailsDto.BirthDate),
            _dateTimeProvider.UtcNow).Value;

        var address = Address.Create(
            command.AddressDto.Street,
            command.AddressDto.City,
            command.AddressDto.State,
            command.AddressDto.ZipCode).Value;
        
        var speciesId = SpeciesId.Create(command.AnimalTypeDto.SpeciesId);

        var isSpeciesExist = await _readDbContext.Species
            .FirstOrDefaultAsync(s => s.Id == command.AnimalTypeDto.SpeciesId, cancellationToken);
        if (isSpeciesExist is null)
            return Errors.General.NotFound();
        
        var isBreedExist = await _readDbContext.Breeds
            .FirstOrDefaultAsync(s => s.Id == command.AnimalTypeDto.BreedId, cancellationToken);
        if (isBreedExist is null)
            return Errors.General.NotFound();
        
        var animalType = new AnimalType(speciesId, command.AnimalTypeDto.BreedId);

        var requisites =
            new ValueObjectList<Requisite>(command.RequisiteDtos
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
            requisites);

        var result =volunteerResult.Value.UpdatePet(
            petId,
            name,
            petPhysicCharacteristics,
            petDetails,
            address,
            phoneNumber,
            helpStatus,
            animalType,
            requisites);

        if (result.IsFailure)
            return result.Errors;
        
        //await _volunteerRepository.Save(volunteerResult.Value, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("added pet with id {petId} to volunteer with id {volunteerId}", petId.Id, volunteerResult.Value.Id.Id);
        
        return pet.Id.Id;
    }
}