using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.BanUser;

public class BanUserCommandValidator: AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(b => b.UserId)
            .NotEmpty()
            .WithError(Errors.General.Null("user id"));
        
        RuleFor(b => b.RelationId)
            .NotEmpty()
            .WithError(Errors.General.Null("relation id"));
    }
}