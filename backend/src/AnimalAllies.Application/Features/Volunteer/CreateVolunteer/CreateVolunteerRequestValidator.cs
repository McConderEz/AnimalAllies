using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public class CreateVolunteerRequestValidator: AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(x => x.FullName)
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.SecondName, x.Patronymic));

        RuleFor(x => x.Description)
            .MustBeValueObject(VolunteerDescription.Create);

        RuleFor(x => x.WorkExperience)
            .MustBeValueObject(WorkExperience.Create);

        RuleFor(x => x.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(x => x.Email)
            .MustBeValueObject(Email.Create);

        RuleForEach(x => x.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Title, x.Url));
        
        RuleForEach(x => x.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));

    }
}