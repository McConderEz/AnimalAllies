using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetPetById;

public class GetPetByIdQueryValidator: AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdQueryValidator()
    {
        RuleFor(v => v.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("pet id"));
    }
}