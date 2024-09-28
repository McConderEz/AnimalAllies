﻿using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdatePet;

public class UpdatePetCommandValidator: AbstractValidator<UpdatePetCommand>
{
    public UpdatePetCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer id"));
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("pet id"));
        
        RuleFor(p => p.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(p => p.HelpStatus)
            .MustBeValueObject(HelpStatus.Create);

        RuleFor(p => p.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(p => p.AddressDto)
            .MustBeValueObject(p => Address.Create(p.Street, p.City, p.State, p.ZipCode));

        RuleFor(p => p.PetDetailsDto.Description)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid(nameof(PetDetails.Description)));
        
        RuleFor(p => p.PetPhysicCharacteristicsDto)
            .MustBeValueObject(p => PetPhysicCharacteristics.Create(
                p.Color,
                p.HealthInformation,
                p.Weight,
                p.Height,
                p.IsCastrated,
                p.IsVaccinated));
        
        RuleForEach(x => x.RequisiteDtos)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));

    }
}