using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Commands.DeleteVolunteer;

public class DeleteVolunteerValidator: AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("id cannot be empty");
    }
}