using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteerById;

public class GetVolunteerByIdQueryValidator : AbstractValidator<GetVolunteerByIdQuery>
{
    public GetVolunteerByIdQueryValidator()
    {
        RuleFor(v => v.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("volunteer id"));
    }
}