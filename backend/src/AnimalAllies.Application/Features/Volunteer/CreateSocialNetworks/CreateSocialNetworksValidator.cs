using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.CreateSocialNetworks;

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