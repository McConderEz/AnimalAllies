using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Species.Commands.CreateSpecies;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .MaximumLength(Constraints.MAX_VALUE_LENGTH)
            .WithError(Errors.General.ValueIsInvalid("name"));
    }
}