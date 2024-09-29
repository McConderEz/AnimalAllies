using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Commands.UpdatePetStatus;

public class UpdatePetStatusCommandValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusCommandValidator()
    {
        RuleFor(p => p.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("volunteer id"));
        
        RuleFor(p => p.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("pet id"));

        RuleFor(p => p.HelpStatus)
            .NotEmpty()
            .Must(p => Constraints.HELP_STATUS_PET_FROM_VOLUNTEER.Contains(p))
            .WithError(Errors.General.ValueIsInvalid("help status"));

    }
}