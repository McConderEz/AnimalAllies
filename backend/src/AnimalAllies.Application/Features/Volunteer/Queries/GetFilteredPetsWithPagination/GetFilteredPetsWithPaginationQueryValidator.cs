using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationQueryValidator : AbstractValidator<GetFilteredPetsWithPaginationQuery>
{
    public GetFilteredPetsWithPaginationQueryValidator()
    {
        RuleFor(v => v.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("volunteer id"));
    }
}
