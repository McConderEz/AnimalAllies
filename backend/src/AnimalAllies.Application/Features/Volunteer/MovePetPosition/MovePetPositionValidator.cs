using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.MovePetPosition;

public class MovePetPositionValidator: AbstractValidator<MovePetPositionCommand>
{
    public MovePetPositionValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleFor(p => p.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));

        RuleFor(p => p.Position)
            .MustBeValueObject(x => Position.Create(x.Value));
    }
}