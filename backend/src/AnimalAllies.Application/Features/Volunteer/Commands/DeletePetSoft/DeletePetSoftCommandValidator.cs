using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeletePetSoft;

public class DeletePetSoftCommandValidator : AbstractValidator<DeletePetSoftCommand>
{
    public DeletePetSoftCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer id"));
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("pet id"));
    }
}