using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Create;

public class CreateRequisitesValidator: AbstractValidator<CreateRequisitesRequest>
{
    public CreateRequisitesValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id cannot be null");
        
        RuleForEach(x => x.Dto.Requisites)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));
    }
}