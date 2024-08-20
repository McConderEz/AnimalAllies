using System.Xml.Linq;
using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Constraints;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateVolunteerRequestValidator: AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(x => new { x.FirstName, x.SecondName, x.Patronymic })
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.SecondName, x.Patronymic));

        RuleFor(x => x.Description)
            .MustBeValueObject(VolunteerDescription.Create);

        RuleFor(x => x.WorkExperience)
            .MustBeValueObject(WorkExperience.Create);

        RuleFor(x => x.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(x => x.Email)
            .MustBeValueObject(Email.Create);

        RuleFor(x => x.SocialNetworks)
            .ForEach(validator =>
                validator.ChildRules(socialNetwork =>
                {
                    socialNetwork.RuleFor(x => new { x.title, x.url })
                        .MustBeValueObject(x => SocialNetwork.Create(x.title, x.url));
                }));
        
        RuleFor(x => x.Requisites)
            .ForEach(validator =>
                validator.ChildRules(requisite =>
                {
                    requisite.RuleFor(x => new { x.title, x.description })
                        .MustBeValueObject(x => Requisite.Create(x.title, x.description));

                }));

    }
}