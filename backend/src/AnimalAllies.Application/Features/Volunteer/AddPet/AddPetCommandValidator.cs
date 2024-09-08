using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.AddPet;

public class AddPetCommandValidator: AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(p => p.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(p => p.HelpStatus)
            .MustBeValueObject(HelpStatus.Create);

        RuleFor(p => p.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(p => p.Address)
            .MustBeValueObject(p => Address.Create(p.Street, p.City, p.State, p.ZipCode));

        RuleFor(p => p.PetDetails.Description)
            .NotEmpty().WithError(Errors.General.ValueIsInvalid(nameof(PetDetails.Description)));
        
        RuleFor(p => p.PetPhysicCharacteristics)
            .MustBeValueObject(p => PetPhysicCharacteristics.Create(
                p.Color,
                p.HealthInformation,
                p.Weight,
                p.Height,
                p.IsCastrated,
                p.IsVaccinated));
        
        RuleForEach(x => x.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));

    }
}