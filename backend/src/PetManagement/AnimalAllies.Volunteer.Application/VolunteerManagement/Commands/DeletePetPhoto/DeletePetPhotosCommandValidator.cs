using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using FluentValidation;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.DeletePetPhoto;

public class DeletePetPhotosCommandValidator: AbstractValidator<DeletePetPhotosCommand>
{
    public DeletePetPhotosCommandValidator()
    {
        RuleFor(p => p.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(p => p.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));

    }
}
