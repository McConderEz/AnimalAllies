using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Delete;

public class DeleteVolunteerValidator: AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("id cannot be empty");
    }
}