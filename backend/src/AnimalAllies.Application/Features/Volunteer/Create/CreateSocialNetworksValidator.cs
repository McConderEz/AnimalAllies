using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public class CreateSocialNetworksValidator: AbstractValidator<CreateSocialNetworksRequest>
{
    public CreateSocialNetworksValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
        
        RuleForEach(x => x.Dto.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));

    }
}