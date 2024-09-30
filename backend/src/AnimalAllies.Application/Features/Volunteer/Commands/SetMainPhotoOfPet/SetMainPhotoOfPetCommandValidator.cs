using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Commands.SetMainPhotoOfPet;

public class SetMainPhotoOfPetCommandValidator: AbstractValidator<SetMainPhotoOfPetCommand>
{
    public SetMainPhotoOfPetCommandValidator()
    {
        RuleFor(p => p.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(p => p.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleFor(p => p.Path)
            .Must(p => Constraints.Extensions.Contains(Path.GetExtension(p))
                       && Path.GetFileNameWithoutExtension(p).Length > 0);

    }
}
