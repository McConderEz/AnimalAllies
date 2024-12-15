﻿using AnimalAllies.Core.Validators;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using FluentValidation;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetPetsBySpeciesId;

public class GetPetsBySpeciesIdQueryValidator : AbstractValidator<GetPetsBySpeciesIdQuery>
{
    public GetPetsBySpeciesIdQueryValidator()
    {
        RuleFor(p => p.SpeciesId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("species id"));
    }
}