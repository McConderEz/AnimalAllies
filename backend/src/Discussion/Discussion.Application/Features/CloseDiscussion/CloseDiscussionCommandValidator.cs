using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace Discussion.Application.Features.CloseDiscussion;

public class CloseDiscussionCommandValidator: AbstractValidator<CloseDiscussionCommand>
{
    public CloseDiscussionCommandValidator()
    {
        RuleFor(p => p.DiscussionId)
            .NotEmpty()
            .WithError(Errors.General.Null("discussion id"));
                
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithError(Errors.General.Null("user id"));
        
    }
}