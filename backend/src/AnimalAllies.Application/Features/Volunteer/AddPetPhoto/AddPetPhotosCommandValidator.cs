using AnimalAllies.Application.Validators;
using AnimalAllies.Domain.Shared;
using FluentValidation;

namespace AnimalAllies.Application.Features.Volunteer.AddPetPhoto;

public class AddPetPhotosCommandValidator: AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        RuleFor(p => p.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(p => p.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));

        //TODO: Валидация файлов + установка главной 
        RuleFor(p => p.Photos)
            .NotNull();
    }
}