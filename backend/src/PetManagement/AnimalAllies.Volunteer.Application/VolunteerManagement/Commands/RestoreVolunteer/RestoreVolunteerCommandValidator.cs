using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.RestoreVolunteer;

public class RestoreVolunteerCommandValidator : AbstractValidator<RestoreVolunteerCommand>
{
    public RestoreVolunteerCommandValidator()
    {
        RuleFor(v => v.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.Null("volunteer id"));
    }
}