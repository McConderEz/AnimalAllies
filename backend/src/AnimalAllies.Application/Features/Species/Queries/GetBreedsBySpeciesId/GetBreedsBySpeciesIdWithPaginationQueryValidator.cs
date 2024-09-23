using AnimalAllies.Application.Features.Species.Queries.GetSpeciesWithPagination;
using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Species.Queries.GetBreedsBySpeciesId;


public class GetBreedsBySpeciesIdWithPaginationQueryValidator : AbstractValidator<GetBreedsBySpeciesIdWithPaginationQuery>
{
    public GetBreedsBySpeciesIdWithPaginationQueryValidator()
    {
        RuleFor(b => b.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("species id"));
        
        RuleFor(s => s.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page"));
        
        RuleFor(s => s.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page size"));
    }
}