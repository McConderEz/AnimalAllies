using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared;
using FluentValidation;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.DeleteBanUser;

public class DeleteBannedUserCommandValidator: AbstractValidator<DeleteBannedUserCommand>
{
    public DeleteBannedUserCommandValidator()
    {
        RuleFor(b => b.UserId)
            .NotEmpty()
            .WithError(Errors.General.Null("user id"));
    }
}