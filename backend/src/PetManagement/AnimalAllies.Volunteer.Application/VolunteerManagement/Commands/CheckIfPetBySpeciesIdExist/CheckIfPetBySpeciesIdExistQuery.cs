using AnimalAllies.Core.Abstractions;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CheckIfPetBySpeciesIdExist;

public record CheckIfPetBySpeciesIdExistQuery(Guid Id) : IQuery;