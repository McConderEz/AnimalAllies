using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace Discussion.Application.Features.Queries;

public class GetDiscussionByRelationIdQueryValidator: AbstractValidator<GetDiscussionByRelationIdQuery>
{
    public GetDiscussionByRelationIdQueryValidator()
    {
        RuleFor(d => d.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page size"));

        RuleFor(d => d.RelationId)
            .NotEmpty()
            .WithError(Errors.General.Null("relation id"));
        
    }
}