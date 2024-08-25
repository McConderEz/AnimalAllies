using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.ValueObjects;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateSocialNetworksValidator: AbstractValidator<CreateSocialNetworksRequest>
{
    public CreateSocialNetworksValidator()
    {
        RuleForEach(x => x.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));

    }
}