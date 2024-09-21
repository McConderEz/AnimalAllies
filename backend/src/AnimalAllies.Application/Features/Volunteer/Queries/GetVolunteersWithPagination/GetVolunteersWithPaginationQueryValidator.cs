using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationQueryValidator : AbstractValidator<GetFilteredVolunteersWithPaginationQuery>
{
    public GetVolunteersWithPaginationQueryValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page"));
        
        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page size"));
    }  
}