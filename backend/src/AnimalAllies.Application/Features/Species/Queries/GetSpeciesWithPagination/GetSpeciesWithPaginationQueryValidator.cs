using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Species.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationQueryValidator : AbstractValidator<GetSpeciesWithPaginationQuery>
{
    public GetSpeciesWithPaginationQueryValidator()
    {
        RuleFor(s => s.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page"));
        
        RuleFor(s => s.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page size"));
    }
}