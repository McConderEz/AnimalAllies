using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Species.Commands.CreateBreed;

public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(b => b.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("species id"));
        
        RuleFor(b => b.Name)
            .NotEmpty()
            .MaximumLength(Constraints.MAX_VALUE_LENGTH)
            .WithError(Errors.General.ValueIsInvalid("name"));
    }
}