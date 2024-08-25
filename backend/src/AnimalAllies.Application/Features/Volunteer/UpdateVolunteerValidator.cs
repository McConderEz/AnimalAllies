using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using AnimalAllies.Domain.ValueObjects;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer;

public class UpdateVolunteerValidator: AbstractValidator<UpdateVolunteerRequest>
{
    public UpdateVolunteerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
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
    }
}