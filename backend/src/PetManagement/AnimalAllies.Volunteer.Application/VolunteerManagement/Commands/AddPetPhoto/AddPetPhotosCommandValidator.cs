using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.AddPetPhoto;

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
                    .Must(ext => Constraints.Extensions.Contains(Path.GetExtension(ext)))
                    .NotEmpty().WithError(Error.Null("filename.is.null", "filename cannot be null or empty"));

                p.RuleFor(p => p.Content)
                    .Must(s => s.Length is > 0 and <= 15 * 1024 * 1024).WithError(Error.Null("stream.empty", "stream cannot be empty"));
            });

    }
}
