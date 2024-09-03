using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.DeleteVolunteer;

public class DeleteVolunteerValidator: AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("id cannot be empty");
    }
}