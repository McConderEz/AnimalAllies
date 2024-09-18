using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Commands.AddPetPhoto;

public class AddPetPhotosCommandValidator: AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        RuleFor(p => p.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(p => p.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));

        RuleForEach(p => p.Photos)
            .ChildRules(p =>
            {
                p.RuleFor(p => p.FileName)
                    .NotEmpty().WithError(Error.Null("filename.is.null", "filename cannot be null or empty"));

                p.RuleFor(p => p.Content)
                    .Must(s => s.Length > 0).WithError(Error.Null("stream.empty", "stream cannot be empty"));
            });

    }
}