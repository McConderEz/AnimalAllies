using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateVolunteerRequestValidator: AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty().WithMessage("FirstName cannot be empty")
            .MaximumLength(Constraints.MAX_VALUE_LENGTH);
        
        RuleFor(x => x.SecondName)
            .NotNull()
            .NotEmpty().WithMessage("SecondName cannot be empty")
            .MaximumLength(Constraints.MAX_VALUE_LENGTH);
        
        RuleFor(x => x.Patronymic)
            .NotNull()
            .NotEmpty().WithMessage("Patronymic cannot be empty")
            .MaximumLength(Constraints.MAX_VALUE_LENGTH);
        
        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty().WithMessage("Description cannot be empty")
            .MaximumLength(Constraints.MAX_DESCRIPTION_LENGTH);

        RuleFor(x => x.WorkExperience)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PhoneNumber)
            .Matches(PhoneNumber.ValidationRegex);
        
        RuleFor(x => x.Email)
            .Matches(Email.ValidationRegex);

        RuleFor(x => x.SocialNetworks)
            .ForEach(validator =>
                validator.ChildRules(socialNetwork =>
                {
                    socialNetwork.RuleFor(x => x.name)
                        .NotNull()
                        .NotEmpty()
                        .MaximumLength(Constraints.MAX_VALUE_LENGTH);
                    socialNetwork.RuleFor(x => x.url)
                        .NotNull()
                        .NotEmpty()
                        .MaximumLength(Constraints.MAX_URL_LENGTH);
                }));
        
        RuleFor(x => x.Requisites)
            .ForEach(validator =>
                validator.ChildRules(requisite =>
                {
                    requisite.RuleFor(x => x.title)
                        .NotNull()
                        .NotEmpty()
                        .MaximumLength(Constraints.MAX_VALUE_LENGTH);
                    requisite.RuleFor(x => x.description)
                        .NotNull()
                        .NotEmpty()
                        .MaximumLength(Constraints.MAX_DESCRIPTION_LENGTH);
                }));

    }
}