using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Models.Volunteer;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.UpdateVolunteer;

public class UpdateVolunteerValidator: AbstractValidator<UpdateVolunteerRequest>
{
    public UpdateVolunteerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleFor(x => x.Dto.FullName)
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.SecondName, x.Patronymic));

        RuleFor(x => x.Dto.Description)
            .MustBeValueObject(VolunteerDescription.Create);

        RuleFor(x => x.Dto.WorkExperience)
            .MustBeValueObject(WorkExperience.Create);

        RuleFor(x => x.Dto.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleFor(x => x.Dto.Email)
            .MustBeValueObject(Email.Create);
    }
}