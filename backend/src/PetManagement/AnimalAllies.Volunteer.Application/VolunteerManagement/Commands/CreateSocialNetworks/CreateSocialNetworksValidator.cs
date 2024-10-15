using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using AnimalAllies.Volunteer.Domain.VolunteerManagement.ValueObject;
using FluentValidation;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CreateSocialNetworks;

public class CreateSocialNetworksValidator: AbstractValidator<CreateSocialNetworksCommand>
{
    public CreateSocialNetworksValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
        
        RuleForEach(x => x.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));

    }
}