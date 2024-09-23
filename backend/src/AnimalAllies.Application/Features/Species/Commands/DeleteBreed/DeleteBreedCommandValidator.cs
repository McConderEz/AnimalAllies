using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Species.Commands.DeleteBreed;

public class DeleteBreedCommandValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedCommandValidator()
    {
        RuleFor(b => b.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("species id"));
        
        RuleFor(b => b.BreedId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("breed id"));
    }
}