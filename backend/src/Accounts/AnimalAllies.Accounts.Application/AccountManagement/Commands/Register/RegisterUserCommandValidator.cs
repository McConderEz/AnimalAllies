using System.Text.RegularExpressions;
using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using FluentValidation;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.Register;

public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .Must(r => Constraints.ValidationRegex.IsMatch(r))
            .WithError(Errors.General.ValueIsInvalid("email"));

        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(Constraints.MIN_LENGTH_PASSWORD)
            .WithError(Errors.General.ValueIsInvalid("password"));

        RuleFor(r => r.UserName)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("username"));

        RuleForEach(r => r.SocialNetworkDtos)
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Url));

        RuleFor(r => r.FullNameDto)
            .MustBeValueObject(f => FullName.Create(f.FirstName, f.SecondName,f.Patronymic));
    }
}