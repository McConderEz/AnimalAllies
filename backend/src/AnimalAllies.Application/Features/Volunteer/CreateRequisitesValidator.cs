using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer;

public class CreateRequisitesValidator: AbstractValidator<CreateRequisitesRequest>
{
    public CreateRequisitesValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        
        RuleForEach(x => x.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));
    }
}