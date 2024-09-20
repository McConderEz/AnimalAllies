using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationQueryValidator : AbstractValidator<GetFilteredVolunteersWithPaginationQuery>
{
    public GetVolunteersWithPaginationQueryValidator()
    {
        RuleFor(v => v.Page)
            .Must(p => p > 0)
            .WithError(Errors.General.ValueIsInvalid("page"));
        
        RuleFor(v => v.PageSize)
            .Must(p => p > 0)
            .WithError(Errors.General.ValueIsInvalid("page size"));
    }  
}